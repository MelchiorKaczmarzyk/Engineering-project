import { HttpClient } from "@angular/common/http";
import { Component, OnDestroy } from "@angular/core";
import ISessionPost from "../session/ISessionPost";
import { lastValueFrom, map, Observable, startWith, Subscription } from "rxjs";
import { GmForSession } from "../gm/GmForSession";
import { GameSystem } from "../gameSystem/GameSystemModel";
import { GameSystenService } from "../gameSystem/GameSystem-service";
import { FormControl } from "@angular/forms";
import { UserService } from "../UserService";

@Component({
    selector: "session-create",
    templateUrl: "./session-create.component.html",
    styleUrl: "./session-create.component.css"
})
export class SessionCreate implements OnDestroy {
    constructor(private http: HttpClient, private systemService: GameSystenService,
        private userService : UserService) { }
    myControl = new FormControl()
    private addSessionUrl = 'https://localhost:7271/api/sessions/addSession'
    private addSystemUrl = 'https://localhost:7271/api/gameSystems/addSystem'
    subPost!: Subscription
    subSystems!: Subscription
    //validation for fields required!
    title: string = ""
    tags: string = ""
    systemName: string = ""
    gm: GmForSession = new GmForSession
    description: string = ""
    maxNumberOfPlayers: number = 0

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
        if(this.title == "")
            this.emptyTitle = true 
        if(this.title.length > 30)
            this.tooLongTitle
        if(this.systemName == "")
            this.emptySystem = true 
        if(this.systemName.length > 30)
            this.tooLongSystem = true
        if(this.description == "")
            this.emptyDescription = true 
        if(this.description.length > 2000)
            this.tooLongDescription = true
        if(this.tags == "")
            this.emptyTags = true 
        if(this.tags.length > 200)
            this.tooLongTags = true
        if(this.maxNumberOfPlayers < 1)
            this.emptyNumberOfPlayers = true
        if(this.sessionPicDisplay == ""){
                this.wrongPicture = true
                this.sessionPicDisplay = ""
        }
        let sessionPicDisplayToSet = ""
        if (this.sessionPicDisplay == undefined || this.sessionPicDisplay == "" ){
            sessionPicDisplayToSet = ""
        }
        else
        sessionPicDisplayToSet = this.sessionPicDisplay.split(',')[1]
                
        let systemForName = new GameSystem()
        systemForName.name = this.systemName

        var session: ISessionPost = {
            system: systemForName,
            title: this.title,
            tags: this.tags,
            description: this.description,
            picture : sessionPicDisplayToSet,
            maxNumberOfPlayers: this.maxNumberOfPlayers,
            gmUserName : this.userService.getCurrentUser().userName
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
                        this.componentState = 0
                    }
                    if(this.futureResponse.isTitleUnique && this.futureResponse.isSystemNew){
                        this.componentState = 3
                    }
                },
                error: () => { this.componentState = 2 }
        })
    
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