import { HttpClient } from "@angular/common/http"
import { Injectable } from "@angular/core"
import { Observable, tap } from "rxjs"
import { Session } from "./Session"


@Injectable({
    providedIn: 'root'
})
export class SessionsService {
    private sessionsUrl = "https://localhost:7271/api/sessions/sessionService"

    constructor(private http: HttpClient) {}
        getSessions(): Observable<Session[]>{
        return this.http.get<Session[]>(this.sessionsUrl).pipe(
            tap(data => console.log("All", JSON.stringify(data)))
        )
}
}
