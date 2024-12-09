import { HttpClient } from "@angular/common/http";
import { Component, OnDestroy } from "@angular/core";
//import ISessionPost from "../models/ISessionPost";
import { lastValueFrom, map, Observable, startWith, Subscription } from "rxjs";
//import { GmForSession } from "../models/GmForSession";
//import { GameSystem } from "../models/GameSystemModel";
//import { GameSystenService } from "../services/GameSystem-service";
import { FormControl } from "@angular/forms";
import { GameSystenService } from "../../services/game-system.service";
import { UserService } from "../../services/user.service";
import { GmForSession } from "../../models/GmForSession";
import { GameSystem } from "../../models/GameSystem";
import ISessionPost from "../../models/ISessionPost";
import { TagsService } from "../../services/tag-service";
import { TriggersService } from "../../services/trigger-service";
import { VttsService } from "../../services/vtt.service";
import { Tag } from "../../models/Tag";
import { Trigger } from "../../models/Trigger";
import { Vtt } from "../../models/Vtt";
//import { UserService } from "../services/user-service";

@Component({
    selector: "session-create",
    templateUrl: "./session-create.component.html",
    styleUrl: "./session-create.component.css"
})
export class SessionCreate implements OnDestroy {
    constructor(private http: HttpClient, 
        private systemService: GameSystenService,
        private userService : UserService, 
        private tagsService : TagsService,
        private triggersService : TriggersService,
        private vttsService : VttsService) { }

    myControl = new FormControl()
    private addSessionUrl = 'https://localhost:7271/api/sessions/addSession'
    private addSystemUrl = 'https://localhost:7271/api/gameSystems/addSystem'
    subPost!: Subscription
    subSystems!: Subscription
    //validation for fields required!
    title: string = ""
    currentTag : string = ""
    tags: string = ""
    numberOfTags : number = 0
    tooFewTags : boolean = false
    currentTrigger : string = ""
    triggers : string = ""
    numberOfTriggers: number = 0
    tooFewTriggers : boolean = false
    currentVtt : string = ""
    vttIsWrong : boolean = false
    location : string = ""
    locationIsWrong : boolean = false   //kiedy jest puste i sesja stacjonarna, a próbujesz dodać sesję, to ustaw to
    date : Date | null = null
    dateNotNull : Date = new Date()
    dateString : string = ""
    dateIsWrong : boolean = false
    dateStringNumber : string = ""
    systemName: string = ""
    gm: GmForSession = new GmForSession
    description: string = ""
    maxNumberOfPlayers: number = 0

    isSessionOnline : boolean = false

    allTriggers : Trigger[] = []
    filteredTriggers : Trigger[] = []
    allTags : Tag[] = []
    filteredTags : Tag[] = []
    allVtts : Vtt[] = []
    filteredVtts : Vtt[] = []
    systems: GameSystem[] = []
    systemsFiltered!: Observable<GameSystem[]>

    //0 - creating session
    //1 - session created successfuly
    //2 - failed to create session
    //3 - new game system
    componentState : number = 0

    sessionPic? : File
    sessionPicDisplay : string = ""
    showImage : boolean = false

    emptySystemAgain : boolean = false
    tooLongSystemAgain = false

    onDateChange(event: any){
        this.date = event.value
        this.dateNotNull = event.value
    }
    addTag() {
        if(this.tags != "")
            this.tags += `, ${this.currentTag}`
        else
            this.tags += this.currentTag

        this.filterTags(this.currentTag)
        ++this.numberOfTags
    }
    clearTags() {
        this.tags = ""
        this.filteredTags = this.allTags
        this.numberOfTags = 0
    }
    addTrigger() {
        if(this.triggers != "")
            this.triggers += `, ${this.currentTrigger}`
        else
            this.triggers += this.currentTrigger

        this.filterTriggers(this.currentTrigger)
        ++this.numberOfTriggers
    }
    clearTriggers() {
        this.triggers = ""
        this.filteredTriggers = this.allTriggers
        this.numberOfTriggers = 0
    }

    filterTags(tagToFilter : string) {
        this.filteredTags = this.filteredTags.filter(tag => tag.name != tagToFilter)
    }

    filterTriggers(triggerToFilter : string) {
        this.filteredTriggers = this.filteredTriggers.filter(trigger => trigger.name != triggerToFilter)
    }
    addSystemThenSubmit() {}
/*
    addSystemThenSubmit() {
        var systemForName: GameSystem = {
            name: this.systemName
        }
        if (this.systemName == "")
            this.emptySystemAgain = true
        if(this.systemName.length > 30)
            this.tooLongSystemAgain = true
        else {
        if (this.systems.findIndex(s => s.name == this.systemName) == -1){
            var post = this.http.post(this.addSystemUrl, systemForName)
                .subscribe({
                    complete: () => {
                        var session: ISessionPost = {
                            system: systemForName,
                            title: this.title,
                            tags: this.tags,
                            description: this.description,
                            picture : this.sessionPicDisplay.split(',')[1],
                            maxNumberOfPlayers: this.maxNumberOfPlayers,
                            gmUserName : this.userService.getCurrentUser().userName
                        }  
                        this.http.post(this.addSessionUrl, session)
                            .subscribe({
                                next: (res) => { console.log(res) },
                                complete: () => {this.componentState = 1},
                                error: () => { this.componentState = 2 }
                            })
                    }
            })
        }
        else {
            let blob = new Blob([this.sessionPicDisplay])
            let formData = new FormData()
            formData.append('file', blob, this.sessionPic?.name)

            var session: ISessionPost = {
                system: systemForName,
                title: this.title,
                tags: this.tags,
                description: this.description,
                picture : this.sessionPicDisplay.split(',')[1],
                maxNumberOfPlayers: this.maxNumberOfPlayers,
                gmUserName : this.userService.getCurrentUser().userName
            }  
            this.http.post(this.addSessionUrl, session)
                .subscribe({
                    next: (res) => { console.log(res) },
                    complete: () => {this.componentState = 1},
                    error: () => { this.componentState = 2 }
                })
        }
    }
    }
*/
    emptyTitle : boolean = false
    tooLongTitle : boolean = false
    emptySystem : boolean = false
    tooLongSystem : boolean = false
    emptyDescription : boolean = false
    tooLongDescription : boolean = false
    emptyTags : boolean = false
    tooLongTags = false
    emptyNumberOfPlayers : boolean = false
    wrongPicture : boolean = false
    titleIsUnique : boolean = true
    futureResponse : any = null
    onSubmitClicked() {
        let isError : boolean = false
        this.tooLongSystem = false
        this.tooLongTitle = false
        this.tooLongDescription = false
        this.emptyTitle = false
        this.emptySystem = false
        this.emptyDescription = false
        this.emptyTags = false
        this.emptyNumberOfPlayers = false
        this.wrongPicture = false
        this.titleIsUnique = true
        if(this.title == "") {
            this.emptyTitle = true; isError=true} 
        if(this.title.length > 30){
            this.tooLongTitle; isError=true} 
        if(this.systemName == ""){
            this.emptySystem = true; isError=true}  
        if(this.systemName.length > 30){
            this.tooLongSystem = true; isError=true} 
        if(this.description == ""){
            this.emptyDescription = true; isError=true}  
        if(this.description.length > 2000){
            this.tooLongDescription = true; isError=true} 
        if(this.tags == ""){
            this.emptyTags = true; isError=true}  
        if(this.tags.length > 200){
            this.tooLongTags = true; isError=true} 
        if(this.maxNumberOfPlayers < 1){
            this.emptyNumberOfPlayers = true; isError=true} 
        if(this.sessionPicDisplay == ""){
                this.wrongPicture = true
                this.sessionPicDisplay = ""
                isError=true
        }
        let sessionPicDisplayToSet = ""
        if (this.sessionPicDisplay == undefined || this.sessionPicDisplay == "" ){
            sessionPicDisplayToSet = ""
            isError = true
        }
        else
            sessionPicDisplayToSet = this.sessionPicDisplay.split(',')[1]
                
        let systemForName = new GameSystem()
        systemForName.name = this.systemName

        
        if(this.numberOfTags < 2){
            this.tooFewTags = true; isError=true} 
        else
            this.tooFewTags = false
        if(this.numberOfTriggers < 1){
            this.tooFewTriggers = true; isError=true} 
        else
            this.tooFewTriggers = false
        let today = new Date()
        if(this.date == null || this.dateNotNull.getTime() < today.getTime()){
            this.dateIsWrong = true; isError=true}
        else
            this.dateIsWrong = false
        if(this.isSessionOnline == true && this.currentVtt == ""){
            this.vttIsWrong = true; isError=true} 
        else
            this.vttIsWrong = false
        if(this.isSessionOnline == false && this.location == ""){
            this.locationIsWrong = true; isError=true} 
        else
            this.locationIsWrong = false

    if(!isError) {
        var session: ISessionPost = {
            system: systemForName,
            title: this.title,
            tags: this.tags,
            triggers: this.triggers,
            isRemote: this.isSessionOnline,
            location: this.location,
            vtt: this.currentVtt,
            date: this.date?.getTime().toString(),
            description: this.description,
            picture : sessionPicDisplayToSet,
            maxNumberOfPlayers: this.maxNumberOfPlayers,
            gmUserName : this.userService.getCurrentUser().userName,
        }  
        var post = this.http.post(this.addSessionUrl, session)
            .subscribe({
                next: (response : any) => {this.futureResponse = response},
                complete: () => {
                    if(this.futureResponse == null){
                        this.componentState = 1
                    }
                    if (!this.futureResponse.isTitleUnique) {
                        this.titleIsUnique = this.futureResponse.isTitleUnique
        
                        if(this.numberOfTags < 2)
                            this.tooFewTags = true
                        else
                            this.tooFewTags = false
                        if(this.numberOfTriggers < 1)
                            this.tooFewTriggers = true
                        else
                            this.tooFewTriggers = false
                        if(this.date == null)
                            this.dateIsWrong = true
                        else
                            this.dateIsWrong = false
                        if(this.isSessionOnline == true && this.currentVtt == "")
                            this.vttIsWrong = true
                        else
                            this.vttIsWrong = false
                        if(this.isSessionOnline == false && this.location == "")
                            this.locationIsWrong = true
                        else
                            this.locationIsWrong = false

                        this.componentState = 0
                    }
                    if(this.futureResponse.isTitleUnique && this.futureResponse.isSystemNew){
                        this.componentState = 3
                    }
                },
                error: () => {
                    this.componentState = 2

                    isError = false
                    this.tooLongSystem = false
                    this.tooLongTitle = false
                    this.tooLongDescription = false
                    this.emptyTitle = false
                    this.emptySystem = false
                    this.emptyDescription = false
                    this.emptyTags = false
                    this.emptyNumberOfPlayers = false
                    this.wrongPicture = false
                    this.titleIsUnique = true
                    this.tooFewTags = false
                    this.tooFewTriggers = false
                    this.dateIsWrong = false
                    this.vttIsWrong = false
                    this.locationIsWrong = false
                }
        })
    }
    
    }
    setState0() {
        this.componentState = 0
        this.description = ""
        this.title = ""
        this.tags = ""
        this.systemName = ""
        this.maxNumberOfPlayers = 0
        this.showImage = false
        this.sessionPicDisplay = ""
    }

    onFileChanged(event : any) {
        const files = event.target.files
        if (files.length === 0)
            return;
    
        const mimeType = files[0].type
    
        const reader = new FileReader()
        this.sessionPic = files;
        reader.readAsDataURL(files[0])
        reader.onload = (_event) => { 
            var lol = reader.result
            console.log(typeof(lol))
            console.log(lol)
            this.sessionPicDisplay = String(reader.result)
        }
        this.showImage = true
    }

    ngOnInit(): void {
        this.subSystems = this.systemService.getSystems().subscribe({
            next: systems => {
                //max 3 or 5 or something like
                this.systems = systems
            }
        })
        this.systemsFiltered = this.myControl.valueChanges.pipe(
            startWith(''),
            map(value => this._filter(value || ''))
        )
       this.tagsService.getTags().subscribe({
        next: tags => {
            this.allTags = tags
            this.filteredTags = tags
       }})
       this.triggersService.getTriggers().subscribe({
        next: triggers => {
            this.allTriggers = triggers
            this.filteredTriggers = triggers
        }
       })
       this.vttsService.getVtts().subscribe({
        next: vtts => {
            this.allVtts = vtts
            this.filteredVtts = vtts
        }
       })
    }   
    private _filter(value: string): GameSystem[] {
        const filterValue = value.toLowerCase()
    
        return this.systems.filter((system, index) => system.name.toLowerCase().includes(filterValue) &&
            index<6)
      }

    ngOnDestroy(): void {
        this.subPost?.unsubscribe
        this.subSystems?.unsubscribe
    }
}