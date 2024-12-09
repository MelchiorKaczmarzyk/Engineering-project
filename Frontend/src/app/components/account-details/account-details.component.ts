import { Component, OnDestroy, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Subscription } from "rxjs";
import { UserService } from "../../services/user.service";
import { UserModel } from "../../models/User";

@Component({
    selector: "account-details",
    templateUrl: "./account-details.component.html",
    styleUrls: ["./account-details.component.css"],
})
export class AccountDetails implements OnInit, OnDestroy{
    constructor(private http : HttpClient, private userService : UserService) {}

    patchUserNameUrl = "https://localhost:7271/api/users/userName"
    patchEmailUrl = "https://localhost:7271/api/users/email"
    patchProfilePictureUrl = "https://localhost:7271/api/users/profilePicture"
    patchDiscordUrl = "https://localhost:7271/api/users/discord"

    forgotPasswordUrl = "https://localhost:7271/api/users/sendMail"
    checkTokenUrl = "https://localhost:7271/api/users/checkToken"
    patchPasswordUrl = "https://localhost:7271/api/users/password"

    //0 - podgląd szczegółów konta
    //1 - edytowanie zdjęcia
    //2 - zdjęcie udało się edytować
    //3 - zdjecie nie udało się edytować
    //4 - edytowanie maila
    //5 - udało sie
    //6 - nie udało się
    //7 - edytowanie nicku
    //8 - udało się
    //9 - nie udało się
    //10 - na pewno delete?
    //11 - udało delete
    //12 - nie udało delete
    //13 - wylogowano
    //14 - eytowanie discord
    //15 - udało się edyt discord
    //16 - nie udało się edit discord
    //17 - podaj hasło, żeby usunąć konto

    //18 - zapomniałem hasła
    //19 - nie udało się wysłac maila
    //20 - zmień hasło
    //21 - udało się zmienić hasło
    //22 - nie udało się zmienić hasła
    //23 - nie można zweryfikować poprawności kodu
    componentState : number = 0

    //Po kliknięciu tych rzeczy tekst zamienia się na input
    onEditPictureClicked() {
        this.componentState = 1
    }
    onEditUserNameClicked() {
        this.componentState = 4
    }
    onEditEmailClicked() {
        this.componentState = 7
    }
    onEditDiscordClicked() {
        this.componentState = 14
    }

    goToDeleteAccount() {
        this.componentState = 10
    }

    returnToDetails() {
        this.emptyUserName = false
        this.uniqueUserName = true
        this.futureResponseName = null

        this.emptyEmail = false
        this.uniqueEmail = true
        this.futureResponseEmail = null

        this.wrongPicture = false
        this.futureResponsePicture = null

        this.emptyDiscord = false
        this.futureResponseDiscord = null

        this.componentState = 0
    }

    newUserName = ""
    emptyUserName = false
    uniqueUserName = true
    tooLongUserName : boolean = false
    futureResponseName : any = null
    updateUserName(){
        this.emptyUserName = false
        this.uniqueUserName = true

        if(this.newUserName.length > 15)
            this.tooLongUserName = true 


        this.http.patch(this.patchUserNameUrl,{
            userName: this.newUserName,
            email: this.currentUser.email
        }).subscribe({
            next: (response : any) => {this.futureResponseName = response},
            complete: () => {
                if (this.futureResponseName == null){
                    this.userService.updateUser(this.currentUser)
                    this.componentState = 8
                }
                else {
                    this.emptyUserName = this.futureResponseName.userNameEmpty
                    this.uniqueUserName = this.futureResponseName.userNameUnique
                    this.componentState = 7
                }
            },
            error: () => {
                console.log("error")
                this.componentState = 9
            }
        })
    }
    newEmail = ""
    emptyEmail = false
    uniqueEmail = true
    futureResponseEmail : any = null
    updateEmail(){
        this.emptyEmail = false
        this.uniqueUserName = true
        this.http.patch(this.patchEmailUrl,{
            email : this.newEmail,
            userName : this.currentUser.userName
        }).subscribe({
            next: (response : any) => {this.futureResponseEmail = response},
            complete: () => {
                if (this.futureResponseEmail == null){
                    this.userService.updateUser(this.currentUser)
                    this.componentState = 5
                }
                else {
                    this.emptyEmail = this.futureResponseName.emailEmptyOrWrong
                    this.uniqueEmail = this.futureResponseName.emailUnique
                    this.componentState = 4
                }
            },
            error: () => {
                console.log("error")
                this.componentState = 6
            }
        })
    }

    wrongPicture = false
    futureResponsePicture : any = null
    updatePicture(){
        this.wrongPicture = false
        var profilePicDisplayValue
        if(this.profilePicDisplay == undefined || this.profilePicDisplay == null
        )
            profilePicDisplayValue = ""
        else
            profilePicDisplayValue = this.profilePicDisplay.split(',')[1]

        this.http.patch(this.patchProfilePictureUrl,{
            userName: this.currentUser.userName,
            profilePic: profilePicDisplayValue
        }).subscribe({
            next: (response : any) => {this.futureResponsePicture = response},
            complete: () => {
                if (this.futureResponsePicture == null){
                this.userService.updateUser(this.currentUser)
                this.componentState = 2
                }
                else {
                    this.wrongPicture = this.futureResponsePicture.wrongPicture
                    this.componentState = 1
                }
            },
            error: () => {
                console.log("error")
                this.componentState = 3
            }
        })
    }

    newDiscord : string = ""
    tooLongDiscord : boolean = false
    emptyDiscord = false
    futureResponseDiscord : any = null
    updateDiscord() {
        this.emptyDiscord = false

        if(this.newDiscord.length > 20)
            this.tooLongDiscord = true 

        this.http.patch(this.patchDiscordUrl,{
            userName: this.currentUser.userName,
            discord: this.newDiscord
        }).subscribe({
            next: (response : any) => {this.futureResponseDiscord = response},
            complete: () => {
                if(this.futureResponseDiscord == null){
                    this.userService.updateUser(this.currentUser)
                    this.componentState = 15
                }
                else {
                    this.emptyDiscord = this.futureResponseDiscord.emptyDiscord
                }
            },
            error: () => {
                console.log("error")
                this.componentState = 16
            }
        })
    }

    resetCode : string = ""
    wrongCode : boolean = false
    forgotPassword() {
        this.http.post(this.forgotPasswordUrl,{
            emailTo: this.currentUser.email,
            emailSubject: "Password reset",
            emailBody: ""
        }).subscribe({
            complete: () => { this.componentState = 18 },
            error: () => { this.componentState = 19 }
        })  
    }

    goToChangePassword() {
        let responseStored : any
        this.http.post(this.checkTokenUrl,{
            Email: this.currentUser.email,
            ResetCode: this.resetCode,
            NewPassword: ""
        }).subscribe({
            next: (response : any) => {
                responseStored = response
            },
            complete: () => {
                if (responseStored.codeIsCorrect) {
                    this.componentState = 20
                    this.wrongCode = false
                }
                else {
                    this.wrongCode = true
                    this.componentState = 18
                }
                },
            error: () => { this.componentState = 23 }
        })  
    }

    newPassword : string = ""
    newPasswordRepeat : string = ""
    wrongPassword: boolean = false
    emptyPassword: boolean = false
    passwordsNotMatch : boolean = false
    error : boolean = false
    updatePassword(){
        if (this.newPassword == "") {
            this.emptyPassword = true
            this.error = true
        }
        if (!this.isPasswordValid(this.newPassword)) {
            this.wrongPassword = true
            this.error = true
        }
        if (this.newPassword != this.newPasswordRepeat) {
            this.passwordsNotMatch = true
            this.error = true
        }
        if (!this.error) {
            this.wrongPassword = false
            this.emptyPassword = false
            this.passwordsNotMatch = false
            this.error = false

            let responseStored : any
            this.http.patch(this.patchPasswordUrl, {
                Email: this.currentUser.email,
                ResetCode: this.resetCode,
                NewPassword: this.newPassword
        }).subscribe({
            next: (response : any) => {
                responseStored = response
            },
            complete: () => { 
                if(responseStored.passwordIsCorrect) {
                    this.componentState = 21
                    this.wrongPassword = false
                }
                else {
                    this.componentState = 20
                    this.wrongPassword = true
                }
            },
            error: () => { 
                this.componentState = 22
            }
        }) 
        }
    }
    isPasswordValid(input: string): boolean {
    const hasCapitalLetter = /[A-Z]/.test(input);
    const hasNonCapitalLetter = /[a-z]/.test(input);
    const hasNumber = /\d/.test(input);
    const hasSpecialCharacter = /[!@#$%^&*(),.?":{}|<>]/.test(input);
    const isAtLeast8CharactersLong = input.length >= 8;

    return (
        hasCapitalLetter &&
        hasNonCapitalLetter &&
        hasNumber &&
        hasSpecialCharacter &&
        isAtLeast8CharactersLong
    );
}
    profilePic? : File
    profilePicDisplay : any
    showImage: boolean = false
    onFileChanged(event : any) {
        const files = event.target.files;
        if (files.length === 0)
            return;
    
        const mimeType = files[0].type;
    
        const reader = new FileReader();
        this.profilePic = files;
        reader.readAsDataURL(files[0]); 
        reader.onload = (_event) => { 
            this.profilePicDisplay = reader.result; 
        }
        this.showImage = true
    }

    logOutUser() {
        this.userService.logOutUser()
        this.componentState = 13
    }

    deleteAccount() {
        this.userService.deleteUser().subscribe({
            complete: () => {
                this.componentState = 11
                this.userService.logOutUser()
            },
            error: () => {this.componentState = 12}
        })
    }

    userSub? : Subscription
    currentUser : UserModel = new UserModel()
    ngOnInit(): void {
        this.userSub = this.userService.currentUser$.subscribe(value => {
            if(this.userService.userIsLoggedIn){
                this.currentUser = this.userService.getCurrentUser()
            }
        })
    }

    ngOnDestroy() : void {
        this.userSub?.unsubscribe()
    }
}