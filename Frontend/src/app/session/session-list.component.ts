import { Component, OnDestroy, OnInit } from "@angular/core";
import { SessionsService } from "./session-service";
import {  map, Observable, startWith, Subscription, tap } from "rxjs";
import { Session } from "./Session";
import { GameSystenService } from "../gameSystem/GameSystem-service";
import { GameSystem } from "../gameSystem/GameSystemModel";
import { FormControl } from "@angular/forms";
import { HttpClient } from "@angular/common/http";
import { UserService } from "../UserService";
import { UserModel } from "../UserModel";
import { PlayerForSession } from "../player/PlayerForSession";
import { PlayerToDisplay } from "../PlayerToDisplay";
import { GmForSession } from "../gm/GmForSession";

@Component({
    selector: "session-list",
    templateUrl: "./session-list.component.html",
    styleUrls: ["./session-list.component.css"]
})
export class SessionList implements OnInit, OnDestroy {
    constructor(private sessionService : SessionsService, private systemService : GameSystenService,
        private userService : UserService, private http: HttpClient
    ) {}
    myControl = new FormControl()

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
    filterTitle : string = ""
    columnsToDisplay = ["Gm", "System", "Title", "Tags", "Players"]
    
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
        this.filterSessions()
    }
    onClearTagFiltersClicked()
    {
        this.filterTag = ""
        this.filtersTags = []
        this.filtersDisplayedTags = ""
        this.filterSessions()
    }

    sessions : Session[] = []
    filteredSessions : Session[] = []
    systems : GameSystem[] = []
    systemsFiltered! : Observable<GameSystem[]>
    private compareTags(session : Session, tags : string[]) : boolean{
        if(tags.length == 0)
            return true
        else{
        var sessionTags = session.tags.split(",")
        return sessionTags.some(tag => tags.includes(tag))
        }
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
    gmPicture : any
    user : UserModel = new UserModel()
    displaySessionDetails(row: Session) {
        this.sessionRow = row
        this.componentState = 1
        console.log(this.sessionRow)
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
        //get session's author image
        //Tu będę musiał całego gma pobierać, z discordem i nickiem oprócz zdjęcia
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
            complete: () => {this.componentState = 3},
            error: () => {this.componentState = 4}
        })
    }

    onReturnClicked() {
        this.componentState = 0
    }

    private sessionIncludesPlayer (session : Session, name : string) : boolean {
        
        if(session.players.some(p => p.name == name))
            return false;
        else
            return true;
        /*
        for (var player of session.players){
            if (player.name != name)
                return true
            else
                return false
        }
        return true
        */
    }
    
    sub!: Subscription
    subSystems!: Subscription
    subPlayers: Subscription = new Subscription()
    subGm : Subscription = new Subscription()
    playersForSession: PlayerToDisplay[] = []
    
    ngOnInit(): void {
         this.sub = this.sessionService.getSessions().subscribe({
             next: sessions => {
                this.sessions = sessions
                if(this.userService.userIsLoggedIn){
                    this.user = this.userService.getCurrentUser()
                    if(this.user.role == "Player"){
                        this.sessions = sessions.filter(s => 
                            this.sessionIncludesPlayer(s, this.user.userName))
                    }
                    if(this.user.role == "Gm")
                        this.sessions = sessions.filter(s => s.gm.name != this.user.userName)
                 }
                
                this.filteredSessions = this.sessions
                 
             },
         })
         this.subSystems = this.systemService.getSystems().subscribe({
            next: systems => {
                //max 3 or 5 or something like
                this.systems = systems
            }
        })
        this.systemsFiltered = this.myControl.valueChanges.pipe(
            startWith(''),
            map(value => this._filterSystems(value || ''))
        )
    }

     ngOnDestroy(): void {
         this.sub.unsubscribe
         this.subSystems.unsubscribe
         this.subPlayers.unsubscribe
         this.subGm.unsubscribe
     }
}