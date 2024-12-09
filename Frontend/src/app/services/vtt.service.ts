import { HttpClient } from "@angular/common/http"
import { Injectable } from "@angular/core"
import { Observable, tap } from "rxjs"
import { Session } from "../models/Session"
import { Vtt } from "../models/Vtt"


@Injectable({
    providedIn: 'root'
})
export class VttsService {
    //lokalny ip, musisz go sprawdzić ipconfig przed prezentacją
    private url = "https://localhost:7271/api/vtts/vttService"

    constructor(private http: HttpClient) {}
        getVtts(): Observable<Vtt[]>{
        return this.http.get<Vtt[]>(this.url).pipe(
            tap(data => console.log("All", JSON.stringify(data)))
        )
}
}
