import { Component } from "@angular/core";
import { Session } from "../../models/Session";
import { Observable, Subscription, tap } from "rxjs";
import { HttpClient } from "@angular/common/http";

import { UserService } from "../../services/user.service";
import { PlayerForSession } from "../../models/PlayerForSession";
import { GmForSession } from "../../models/GmForSession";
import { UserModel } from "../../models/User";
import { PlayerToDisplay } from "../../models/PlayerToDisplay";
import { SessionsService } from "../../services/session.service";

@Component({
    selector: "my-sessions-player",
    templateUrl: "./my-sessions-player.component.html",
    styleUrls: ["./my-sessions-player.component.css"],
})
export class MySessionsPlayer {
    constructor( private http: HttpClient, private sessionService : SessionsService,
        private userService : UserService) {}

    //0 - list of sessions
    //1 - session details
    //2 - haven't joined any sessions yet
    //3 - u sure leave?
    //4 - leave success
    //5 - leave fail
    componentState : number = 0
    columnsToDisplay = ["Gm", "System", "Title", "Tags", "Players"]
    
    leaveSessionUrl = 'https://localhost:7271/api/players/leaveSession'
    playersForSessionUrl = 'https://localhost:7271/api/players/playersService'
    getSessionPictureUrl : string = "https://localhost:7271/api/sessions/sessionPicture"
    getGmPictureUrl : string = ""
    sessionRow : Session = new Session()
    sessionPicture : any
    gmPicture : any
    getGmForSessionUrl : string = "https://localhost:7271/api/gms/gmForSession"
    gmForSession : GmForSession = new GmForSession()
    user : UserModel = new UserModel()
    playersForSession : PlayerToDisplay[] = []
    playersForSessionLess : PlayerToDisplay[] = []
    noPlayers : boolean = true
    initialLength : number = 0

    day : number = 0
    month : number = 0
    year : number = 0
    displaySessionDetails(row: Session) {
        this.sessionRow = row
        let sessionRowDate = new Date(Number.parseInt(this.sessionRow.date))
        this.day = sessionRowDate.getDate()
        this.month = sessionRowDate.getMonth()+1
        this.year = sessionRowDate.getFullYear()
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
                this.initialLength = this.playersForSession.length
            },
            complete: () => { 
                if(this.playersForSession.length == 0)
                    this.noPlayers == true
                else
                    this.noPlayers == false
                for (var player of this.playersForSession){
                    if(player.name == this.user.userName) {
                        this.user.discord = player.discord
                        var index = this.playersForSession.findIndex(p => p.name == player.name)
                        this.playersForSessionLess = this.playersForSession.splice(index,1)
                    }
                }
            },
            error: () => { console.log("errored")}
        })
    }
    
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

    onReturnClicked() {
        this.componentState = 0
        if (this.filteredSessions.length == 0)
            this.componentState = 2
    }

    goToLeaveSession() {
        this.componentState = 3
    }
    
    subLeaveSession! : Subscription
    leaveSession() {
        this.http.patch(this.leaveSessionUrl, {
            userName : this.user.userName,
            sessionTitle : this.sessionRow.title
        }).subscribe ({
            next : () => {
                this.getSessionsForPlayer(false)
            },
            complete: () => {this.componentState = 4},
            error: () => {this.componentState = 5}
        })
    }

    sessions : Session[] = []
    filteredSessions : Session[] = []
    sub!: Subscription
    subPlayers! : Subscription
    subGm! : Subscription

    getSessionsForPlayer(changeComponentState : boolean) {
        this.sub = this.sessionService.getSessions().subscribe({
            next: sessions => {
                if(this.userService.userIsLoggedIn)
                this.sessions = sessions
            },
            complete: () => {
                let user = this.userService.getCurrentUser()
                this.filteredSessions = this.sessions.filter(s => {
                    for (var p of s.players){
                        if(p.name == user.userName)
                            return true
                        else
                            continue
                    }
                    return false
                }
                )
                if (this.filteredSessions.length == 0 && changeComponentState == true)
                    this.componentState = 2
            }
        })
    }
    ngOnInit(): void {
        this.getSessionsForPlayer(true)
    }
    ngOnDestroy(): void {
        this.sub.unsubscribe()
    }
}