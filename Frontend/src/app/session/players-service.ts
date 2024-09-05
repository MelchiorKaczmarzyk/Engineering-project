import { HttpClient } from "@angular/common/http"
import { PlayerToDisplay } from "../PlayerToDisplay"
import { Injectable } from "@angular/core"
import { Observable, tap } from "rxjs"

@Injectable({
    providedIn: 'root'
})
export class PlayerssService {
    private sessionsUrl = 'https://localhost:7271/api/players/playersService'

    constructor(private http: HttpClient) {}
        getSessions(): Observable<PlayerToDisplay[]>{
        return this.http.get<PlayerToDisplay[]>(this.sessionsUrl).pipe(
            tap(data => console.log("All", JSON.stringify(data)))
        )
}
}