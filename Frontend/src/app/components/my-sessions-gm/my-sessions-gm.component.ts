import { HttpClient } from "@angular/common/http";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { SessionsService } from "../../session/session-service";
import { Session } from "../../session/Session";
import { Observable, Subscription, tap } from "rxjs";
import { UserService } from "../../UserService";
import { GmForSession } from "../../gm/GmForSession";
import { UserModel } from "../../UserModel";
import { PlayerToDisplay } from "../../PlayerToDisplay";

@Component({
    selector: "my-sessions-gm",
    templateUrl: "./my-sessions-gm.component.html",
    styleUrls: ["./my-sessions-gm.component.css"],
})
export class MySessionsGm implements OnInit, OnDestroy{
    constructor( private http: HttpClient, private sessionService : SessionsService,
        private userService : UserService) {}

    //0 - list of sessions
    //1 - session details
    //2 - no sessions added yet
    //3 - u sure delete?
    //4 - delete success
    //5 - delete fail
    componentState : number = 0
    columnsToDisplay = ["System", "Title", "Tags", "Players"]

    deleteSessionUrl = 'https://localhost:7271/api/sessions/deleteSession'
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

    
    subDeleteSession! : Subscription
    deleteSession() {
        {
            this.http.delete(this.deleteSessionUrl, {
                params:{
                    sessionTitle : this.sessionRow.title
                }
            }).subscribe ({
                next : () => {
                    this.getSessionsForGm(false)
                },
                complete: () => {
                    this.componentState = 4
                },
                error: () => {this.componentState = 5}
            })
        }
    }
    goToDeleteSession() {
        this.componentState = 3
    }

    sessions : Session[] = []
    filteredSessions : Session[] = []
    sub!: Subscription
    subGm! : Subscription
    subPlayers! : Subscription
    getSessionsForGm(changeComponentState : boolean) {
        this.sub = this.sessionService.getSessions().subscribe({
            next: sessions => {
                if(this.userService.userIsLoggedIn)
                this.sessions = sessions
            },
            complete: () => {
                let user = this.userService.getCurrentUser()
                this.filteredSessions = this.sessions.filter(s => 
                    s.gm.name == user.userName)
                if (this.filteredSessions.length == 0 && changeComponentState == true)
                    this.componentState = 2
            }
        })
    }
    ngOnInit(): void {
        this.getSessionsForGm(true)
    }
    ngOnDestroy(): void {
        this.sub.unsubscribe()
    }
}