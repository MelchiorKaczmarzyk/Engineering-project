import { HttpClient } from "@angular/common/http"
import { Injectable } from "@angular/core"
import { Observable, tap } from "rxjs"
import { Session } from "../session/Session"
import { GameSystem } from "./GameSystemModel"


@Injectable({
    providedIn: 'root'
})
export class GameSystenService {
    private sessionsUrl = 'https://localhost:7271/api/gameSystems'

    constructor(private http: HttpClient) {}
        getSystems(): Observable<GameSystem[]>{
        return this.http.get<GameSystem[]>(this.sessionsUrl).pipe(
            tap(data => console.log("All", JSON.stringify(data)))
        )
}
}
