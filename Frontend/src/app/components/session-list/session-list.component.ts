import { Component, OnDestroy, OnInit, ViewChild } from "@angular/core";

import {  from, map, Observable, startWith, Subscription, tap } from "rxjs";
import { Session } from "../../models/Session";
import { GameSystenService } from "../../services/game-system.service";
import { GameSystem } from "../../models/GameSystem";
import { FormControl } from "@angular/forms";
import { HttpClient } from "@angular/common/http";
import { UserService } from "../../services/user.service";
import { UserModel } from "../../models/User";
import { PlayerToDisplay } from "../../models/PlayerToDisplay";
import { GmForSession } from "../../models/GmForSession";
import { SessionsService } from "../../services/session.service";
import { MatPaginatorIntl, PageEvent } from "@angular/material/paginator";
import { PaginatorIntl } from "../../services/paginatorIntl.service";
import { MatTable, MatTableDataSource } from "@angular/material/table";
import { TriggersService } from "../../services/trigger-service";
import { TagsService } from "../../services/tag-service";
import { VttsService } from "../../services/vtt.service";
import { Trigger } from "../../models/Trigger";
import { Tag } from "../../models/Tag";
import { Vtt } from "../../models/Vtt";
import { trigger } from "@angular/animations";

@Component({
    selector: "session-list",
    templateUrl: "./session-list.component.html",
    styleUrls: ["./session-list.component.css"],
    providers: [{ provide: MatPaginatorIntl, useClass: PaginatorIntl }]
})
export class SessionList implements OnInit, OnDestroy {
    constructor(private sessionService : SessionsService, 
        private systemService : GameSystenService,
        private userService : UserService, 
        private tagService : TagsService,
        private triggerService : TriggersService,
        private vttService : VttsService,
        private http: HttpClient
    ) {}
    myControl = new FormControl()

    currentTrigger : string = ""
    triggers : string = ""
    currentVtt : string = ""
    vttIsWrong : boolean = false
    location : string = ""
    date : Date | null = null

    isSessionOnline : number = 0
    allTriggers : Trigger[] = []
    filteredTriggers : Trigger[] = []
    allTags : Tag[] = []
    filteredTags : Tag[] = []
    allVtts : Vtt[] = []
    filteredVtts : Vtt[] = []
    systems: GameSystem[] = []
    allSystems : GameSystem[] = []
    // filtering by date, by location and by isOnline

    addTriggerFilter() {
        if(this.triggers != "")
            this.triggers += `, ${this.currentTrigger}`
        else
            this.triggers += this.currentTrigger

        this.filterTriggerFilters(this.currentTrigger)
    }
    clearTriggerFilters() {
        this.triggers = ""
        this.filteredTriggers = this.allTriggers
    }

    filterTriggerFilters(triggerToFilter : string) {
        this.filteredTriggers = this.filteredTriggers.filter(trigger => trigger.name != triggerToFilter)
    }

    filterTagFilters(tagToFilter : string) {
        this.filteredTags = this.filteredTags.filter(tag => tag.name != tagToFilter)
    }

    filterSystemFilters(systemToFilter : string) {
        this.systems = this.systems.filter(tag => tag.name != systemToFilter)
    }

    get pageSize() : number {
        return 2
        //Code for using a proper page size
        /*
        if (this.filteredSessions.length < 11)
            return this.filteredSessions.length
        else
            return 10
        */
    }
    get numberOfPages() : number {
        return Math.ceil(this.filteredSessions.length / this.pageSize)
    }

    currentPageIndex : number = 0
    handlePagination(pageEvent : PageEvent){
        const previousPageIndex = pageEvent.previousPageIndex;
        const currentPageIndex = pageEvent.pageIndex;
    
        if(previousPageIndex != undefined){
            if (currentPageIndex == previousPageIndex + 1) {
            this.onNextPage(currentPageIndex) // Next page
            } else if (currentPageIndex == previousPageIndex - 1) {
            this.onPreviousPage(currentPageIndex); // Previous page
            } else if (currentPageIndex == 0) {
            this.onFirstPage(currentPageIndex); // First page
            } else if (currentPageIndex == this.numberOfPages - 1) {
            this.onLastPage(); // Last page
            }
            this.currentPageIndex = currentPageIndex
        } 
    }

    onNextPage(currentPageIndex : number){
        this.getSessionsForPage(currentPageIndex)
        console.log(this.sessionsForPage)
    }
    onPreviousPage(currentPageIndex : number) {
        this.getSessionsForPage(currentPageIndex)
        console.log(this.sessionsForPage)
    }
    onFirstPage(currentPageIndex : number){
        this.getSessionsForPage(currentPageIndex)
        console.log(this.sessionsForPage)
    }
    //The only different one
    onLastPage(){
        let reminder = this.filteredSessions.length % this.pageSize
        this.sessionsForPage = this.filteredSessions.slice(-reminder,-1)
        this.sessionsForPage.push(this.filteredSessions[this.filteredSessions.length-1])
        console.log(this.sessionsForPage)
        this.filterSessions()
    }

    sessionsForPage : Session[] = []

    getSessionsForPage(currentPageNumber : number) {
        if(this.filteredSessions.length > this.pageSize){
            let firstIndex = currentPageNumber*this.pageSize
            this.sessionsForPage = this.filteredSessions.slice(firstIndex,
                firstIndex+(this.pageSize))
        }
        else
            this.sessionsForPage = this.filteredSessions
        this.filterSessions()
    }
    //0 - session list
    //1 - session details
    //2 - no sessions matches the filters
    //3 - joined session succesfully
    //4 - join failed
    componentState = 0

    anySessions : boolean = true

    private _filterGm = ""
    get filterGm() : string {
        return this._filterGm
    }
    set filterGm(value : string) {
        this._filterGm = value
    }
    private _filterSystem = ""
    get filterSystem() : string {
        return this._filterSystem
    }
    set filterSystem(value : string) {
        this._filterSystem = value
    }
    private _filterTag = ""
    get filterTag() : string {
        return this._filterTag
    }
    set filterTag(value : string) {
        this._filterTag = value
    }
    private _filterTrigger = ""
    get filterTrigger() : string {
        return this._filterTrigger
    }
    set filterTrigger(value : string) {
        this._filterTrigger = value
    }
    filterTitle : string = ""
    columnsToDisplay = ["Gm", "System", "Title", "Tags", "Triggers"]
    
    filtersGm : string[] = []
    filtersSystem : string[] = []
    filtersTags : string[] = []
    filtersDisplayedGm : string = ""
    fitlersDisplayedSystem : string = ""
    filtersDisplayedTags : string = ""
    
    onAddGmFilterClicked()
    {
        if(this.filtersDisplayedGm.length != 0)
            {
                this.filtersDisplayedGm += ", "
            }
        this.filtersDisplayedGm += this.filterGm
        this.filterGm = ""
        
        this.filtersGm = this.filtersDisplayedGm.split(",")
        this.filterSessions()  
    }
    onAddSystemFilterClicked()
    {
        if(this.fitlersDisplayedSystem.length != 0)
            {
                this.fitlersDisplayedSystem += ", "
            }

        this.fitlersDisplayedSystem += this.filterSystem
        this.filterSystemFilters(this.filterSystem)
        this.filterSystem = ""
        
        this.filtersSystem = this.fitlersDisplayedSystem.split(",")
        this.filterSessions()
    }
    onAddTagFilterClicked()
    {
        if(this.filtersDisplayedTags.length != 0)
            {
                this.filtersDisplayedTags += ", "
            }

        this.filtersDisplayedTags += this.filterTag
        this.filterTagFilters(this.filterTag)
        this.filterTag = ""
        
        this.filtersTags = this.filtersDisplayedTags.split(",")
        this.filterSessions()  
    }

    onClearGmFiltersClicked()
    {
        this.filterGm = ""
        this.filtersGm = []
        this.filtersDisplayedGm = ""
        this.filterSessions()
    }
    onClearSystemFiltersClicked()
    {
        this.filterSystem = ""
        this.filtersSystem = []
        this.fitlersDisplayedSystem = ""
        this.systems = this.allSystems
        this.filterSessions()
    }
    onClearTagFiltersClicked()
    {
        this.filterTag = ""
        this.filtersTags = []
        this.filtersDisplayedTags = ""
        this.filteredTags = this.allTags
        this.filterSessions()
    }

    sessions : Session[] = []
    filteredSessions : Session[] = []
    systemsFiltered! : Observable<GameSystem[]>
    private compareTags(session : Session, tags : string[]) : boolean{
        if(tags.length == 0)
            return true
        else{
        var sessionTags = session.tags.split(",")
        return sessionTags.some(tag => tags.includes(tag))
        }
    }
    onDateChange(event: any){
        this.date = event.value
        this.filterSessions()
    }
    filterSessions() {
        this.filteredSessions = this.sessions.filter((session) => (
            this.filtersDisplayedGm
            .includes(session.gm.name) || this.filtersDisplayedGm == "") && (
            this.fitlersDisplayedSystem
            .includes(session.system.name) || this.fitlersDisplayedSystem == "") && (
            session.title
            .includes(this.filterTitle)) && (
            this.compareTags(session, this.filtersTags)))

        this.filteredSessions = this.filteredSessions.filter(session => 
            session.triggers == this.triggers || this.triggers == "")

        if(this.date != null){
        this.filteredSessions = this.filteredSessions.filter(session =>
            Number(session.date) > this.date!.getTime())
        }


        if(this.isSessionOnline != 0){
            if(this.isSessionOnline == 2) {
                this.filteredSessions = this.filteredSessions.filter(session =>
                    session.isRemote == true)
        
                this.filteredSessions = this.filteredSessions.filter(session => 
                    session.vtt == this.currentVtt || this.currentVtt == "")
            }
            if(this.isSessionOnline == 1) {
                this.filteredSessions = this.filteredSessions.filter(session => 
                    session.isRemote == false)
                this.filteredSessions = this.filteredSessions.filter(session =>
                    session.location.toLowerCase().includes(this.location.toLowerCase()) || 
                    this.location.toLowerCase().includes(session.location.toLowerCase()) ||
                    this.location == "")
            }
        }
        this.filteredSessions = this.filteredSessions.filter(s => 
                s.currentNumberOfPlayers < s.maxNumberOfPlayers)
                
        if(this.filteredSessions.length > this.pageSize){
            let firstIndex = this.currentPageIndex*this.pageSize
            this.sessionsForPage = this.filteredSessions.slice(firstIndex,
                firstIndex+(this.pageSize))
            }
        else
            this.sessionsForPage = this.filteredSessions

        if (this.filteredSessions.length == 0)
           this.anySessions = false
        else 
            this.anySessions = true
    }

    getSessionPictureUrl : string = "https://localhost:7271/api/sessions/sessionPicture"
    getGmForSessionUrl : string = "https://localhost:7271/api/gms/gmForSession"
    gmForSession : GmForSession = new GmForSession()
    sessionRow = new Session()
    sessionPicture : any
    sessionRowDate = new Date()
    day : number = 0
    month : number = 0
    year : number = 0
    gmPicture : any
    user : UserModel = new UserModel()
    displaySessionDetails(row: Session) {
        this.sessionRow = row
        this.componentState = 1
        console.log(this.sessionRow)
        this.sessionRowDate = new Date(Number.parseInt(this.sessionRow.date))
        this.day = this.sessionRowDate.getDate()
        this.month = this.sessionRowDate.getMonth()+1
        this.year = this.sessionRowDate.getFullYear()
        if(this.userService.userIsLoggedIn)
            this.user = this.userService.getCurrentUser()
        //get session image
        this.http.get(this.getSessionPictureUrl, 
            {
            params: {
                title: row.title,
            }
        }).subscribe({
            next: (data : any) => {
                this.sessionPicture = "data:image/png;base64,"+String(data.sessionPicture.fileContents)
                this.gmPicture =  "data:image/png;base64,"+String(data.gmPicture.fileContents)
                },
            complete: () => {
                console.log(this.sessionPicture)
            },
            error: () => {
                console.log("session picture error")
            }
        })
        this.subGm = this.getGmForSession(row).subscribe({
            next: gm => {
                this.gmForSession = gm
            },
            complete: () => { console.log("completed")},
            error: () => { console.log("errored")}
        })
        this.subPlayers = this.getPlayersForSession(row).subscribe({
            next: players => {
                this.playersForSession = players
            },
            complete: () => { console.log("completed")},
            error: () => { console.log("errored")}
        })
    }
    private playersForSessionUrl = 'https://localhost:7271/api/players/playersService'
    getPlayersForSession(session : Session): Observable<PlayerToDisplay[]>{
       return this.http.get<PlayerToDisplay[]>(this.playersForSessionUrl, {
           params: {
               title : session.title
           }
       }
       ).pipe(
           tap(data => console.log("All", JSON.stringify(data)))
       )
   }
   getGmForSession(session : Session): Observable<GmForSession>{
    return this.http.get<GmForSession>(this.getGmForSessionUrl, {
        params: {
            title : session.title
        }
    }
    )
}
    private _filterSystems(value: string): GameSystem[] {
        const filterValue = value.toLowerCase()
    
        return this.systems.filter((system, index) => system.name.toLowerCase().includes(filterValue) &&
            index<6)
    }

    //patch
    patchUrl = "https://localhost:7271/api/sessions/addPlayerToSession"
    joinPlayer() {
        this.http.patch(this.patchUrl,{
            playerName: this.user.userName,
            sessionTitle: this.sessionRow.title
        }).subscribe({
            complete: () => {
                this.componentState = 3
                this.filterSessions()
            },
            error: () => {this.componentState = 4}
        })
    }

    get playerIsInSession() {
        for(let player of this.sessionRow.players){
            if(player.name == this.userService.getCurrentUser().userName)
                return true
        }
        return false
    }

    onReturnClicked() {
        this.componentState = 0
        this.sub = this.sessionService.getSessions().subscribe({
            next: sessions => {
               this.sessions = sessions
               /*
               if(this.userService.userIsLoggedIn){
                   this.user = this.userService.getCurrentUser()
                   if(this.user.role == "Player"){
                       this.sessions = sessions.filter(s => 
                           this.sessionIncludesPlayer(s, this.user.userName))
                   }
                   if(this.user.role == "Gm")
                       this.sessions = sessions.filter(s => s.gm.name != this.user.userName)
                }
                   */
                this.filteredSessions = this.sessions.filter(s => 
                   s.currentNumberOfPlayers < s.maxNumberOfPlayers)
            },
            error: () => {this.explore = "error"}
        })
    }
    
    sub!: Subscription
    subSystems!: Subscription
    subPlayers: Subscription = new Subscription()
    subGm : Subscription = new Subscription()
    playersForSession: PlayerToDisplay[] = []
    
    explore : string = "Explore"
    ngOnInit(): void {
         this.sub = this.sessionService.getSessions().subscribe({
             next: sessions => {
                this.sessions = sessions
                /*
                if(this.userService.userIsLoggedIn){
                    this.user = this.userService.getCurrentUser()
                    if(this.user.role == "Player"){
                        this.sessions = sessions.filter(s => 
                            this.sessionIncludesPlayer(s, this.user.userName))
                    }
                    if(this.user.role == "Gm")
                        this.sessions = sessions.filter(s => s.gm.name != this.user.userName)
                 }
                    */
                 this.filteredSessions = this.sessions.filter(s => 
                    s.currentNumberOfPlayers < s.maxNumberOfPlayers)
             },
             complete: () => {
                this.filterSessions()
                this.getSessionsForPage(0)
            },
             error: () => {this.explore = "error"}
         })
         this.subSystems = this.systemService.getSystems().subscribe({
            next: systems => {
                //max 3 or 5 or something like
                this.systems = systems
                this.allSystems = systems
            }
        })
        this.systemsFiltered = this.myControl.valueChanges.pipe(
            startWith(''),
            map(value => this._filterSystems(value || ''))
        )
        this.tagService.getTags().subscribe({
            next: tags => {
                this.allTags = tags
                this.filteredTags = tags
            }
        })
        this.triggerService.getTriggers().subscribe({
            next: triggers => {
                this.allTriggers = triggers
                this.filteredTriggers = triggers
            }
        })
        this.vttService.getVtts().subscribe({
            next: vtts => {
                this.allVtts = vtts
                this.filteredVtts = vtts
            }
        })
        
    }

     ngOnDestroy(): void {
         this.sub.unsubscribe
         this.subSystems.unsubscribe
         this.subPlayers.unsubscribe
         this.subGm.unsubscribe
     }
}