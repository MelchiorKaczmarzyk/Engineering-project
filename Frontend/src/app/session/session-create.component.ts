import { HttpClient } from "@angular/common/http";
import { Component, OnDestroy } from "@angular/core";
import ISessionPost from "./ISessionPost";
import { Observable, Subscription } from "rxjs";
import { GmForSession } from "../gm/GmForSession";

@Component({
    selector: "session-create",
    templateUrl: "./session-create.component.html",
    styleUrl: "./session-create.component.css"
})
export class SessionCreate implements OnDestroy{
    constructor(private http: HttpClient) {}

    private targetUrl = 'https://localhost:7271/api/sessions/addSession'
    sub!: Subscription
    //validation for fields required!
    title : string = ""
    tags : string = ""       //adding tags like in search
    system : string = ""
    gm : GmForSession = new GmForSession   //currently logged in user, not imputed on creation
    description : string = ""
    maxNumberOfPlayers : number = 0

    onSubmitClicked() {
        var session : ISessionPost = {
            system : this.system,
            title : this.title,
            tags : this.tags,
            description : this.description,
            maxNumberOfPlayers : this.maxNumberOfPlayers
        }
        var post = this.http.post<ISessionPost>(this.targetUrl, session)
        .subscribe((res) =>
            {
                console.log(res)
            }
        )
        //return this.http.post<ISessionPost>(this.targetUrl, session).subscribe
    }

    ngOnDestroy(): void {
        this.sub.unsubscribe
    }
}