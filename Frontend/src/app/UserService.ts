import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { UserModel } from "./UserModel";

@Injectable({
    providedIn: 'root'
})
export class UserService {
    constructor(private http: HttpClient) {}
    private urlLogIn : string = "https://localhost:7271/api/users/logInUser"
    private urlUpdate : string = "https://localhost:7271/api/users/relogInUser"
    private urlDelete : string = "https://localhost:7271/api/users/user"

    private _currentUser = new BehaviorSubject<any>(null)
    get currentUser(): any{
        return this._currentUser.value
    }
    set currentUser(value: any) {
        this._currentUser.next(value)
    }
    currentUser$ = this._currentUser.asObservable()

    public userIsLoggedIn : boolean = false

    //Shoud be used after checking if any user is logged in
    getCurrentUser() : UserModel{
        return this.currentUser
    }

    logInUser(emailEntered : string, passwordEntered: string) : Observable<any> {
        return this.http.get(this.urlLogIn, 
            {
            params: {
                email: emailEntered,
                password: passwordEntered
            }
        })
    }


    logOutUser() {
        this.userIsLoggedIn = false
        this.currentUser = null
    }

    //robi to samo, co logowanie, ale jakby loguje drugi raz tego samego użytkownika
    updateUser(user : UserModel) {
        //Zmieniła się dokładnie 1 z 3 rzeczy (email, userName, profilePic)
        //Każda niezmieniona rzecz jednoznacznie identyfikuje użytkownika
        //Muszę znaleźć tego użytkownika, który jest teraz zalogowany po tej rzeczy, która jest nie
        //  zmieniona i wysłać go tutaj z powrotem i przypisać do currentUser
        //TO SIĘ MUSI WYDARZYĆ PO ZAKOŃCZENIU FAKTYCZNEJ ZMIANY NA BACKENDZIE
        this.http.get(this.urlUpdate, {
            params: {
                email: user.email,
                userName: user.userName
            }
        }).subscribe({
            next: (data : any) => {
                this.currentUser = {
                    email : data.email,
                    userName : data.userName,
                    role : data.role,
                    discord : data.discord,
                    profilePicture : "data:image/png;base64,"+
                        String(data.profilePicture.fileContents)
                    }
                    console.log("lol")
                },
            complete: () => {console.log("completed")},
            error: () => {console.log("errored")}
        })

    }

    //usuwa usera całkiem
    deleteUser() : Observable<any> {
        let userNameParam : string = this.currentUser.userName
        return this.http.delete(this.urlDelete, {
            params: {
                userName : userNameParam
            }
        })
    }
}