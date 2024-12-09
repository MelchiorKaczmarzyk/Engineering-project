import { Component, OnDestroy, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { UserService } from "../../services/user.service";
import { UserRegister } from "../../models/UserRegister";

@Component({
    selector: "login-register",
    templateUrl: "./login-register.component.html",
    styleUrls: ["./login-register.component.css"]
})
export class LoginRegister implements OnInit, OnDestroy {
    constructor(private http: HttpClient, private userService : UserService) {}

    urlRegister : string = "https://localhost:7271/api/users/registerUser"
    //0 - start screen and logging in
    //1 - register screen
    //2 - logging error screen
    //3 - logging succesfully screen
    //4 - register error screen
    //5 - register succesfully screen
    componentState : number = 0

    onReturnToHomeClicked() {
        this.componentState = 0
    }

    emailEnteredLogIn : string = ""
    passwordEnteredLogIn : string = ""
    
    emailEnteredRegister : string = ""
    passwordEnteredRegister : string = ""
    repeatPasswordRegister : string = ""
    discordEntered : string = ""
    userNameEntered : string = ""
    roleChosen : string = ""

    profilePic? : File
    profilePicDisplay : any = ""

    roles : String[] = ["Player", "Gm"]

    emptyEmailLogIn : boolean = false
    emptyPasswordLogIn : boolean = false
    emailIsCorrectLogIn : boolean = true
    passwordIsCorrectLogIn : boolean = true
    errorHappenedLogIn : boolean = false
    logIn(registering : boolean) {
        this.emptyEmailLogIn = false
        this.emptyPasswordLogIn = false
        this.emailIsCorrectLogIn = true
        this.passwordIsCorrectLogIn = true
        this.errorHappenedLogIn = false

        if(this.emailEnteredLogIn == ""){
            this.emptyEmailLogIn = true
            this.errorHappenedLogIn = true
        }
        if(this.passwordEnteredLogIn == ""){
            this.emptyPasswordLogIn = true
            this.errorHappenedLogIn = true
        }
        let user : any
        let futureProfilePicture : string = ""
        let response = this.userService.logInUser(this.emailEnteredLogIn, 
            this.passwordEnteredLogIn)
        response.subscribe({
            next: (data : any) => {
                if(data.profilePicture != null)
                    futureProfilePicture = "data:image/png;base64," +
                        String(data.profilePicture.fileContents)

                user = {
                    email : data.email,
                    userName : data.userName,
                    discord : data.discord,
                    profilePicture : futureProfilePicture,
                    role : data.role
                    }
                this.emailIsCorrectLogIn = data.emailIsCorrect
                this.passwordIsCorrectLogIn = data.passwordIsCorrect
                },
            complete: () => {
                if (this.emailIsCorrectLogIn && this.passwordIsCorrectLogIn &&
                    (this.emailEnteredLogIn != "") && (this.passwordEnteredLogIn != "")) {
                        if (this.userService.userIsLoggedIn)
                            this.userService.logOutUser()
                        this.userService.userIsLoggedIn = true
                        this.userService.currentUser = user
                        if(!registering)
                            this.componentState = 3
                    }
                    else {this.componentState = 0 }
                    
            },
            error: () => {
                this.userService.userIsLoggedIn = false
                if (this.emptyEmailLogIn)
                    this.emailIsCorrectLogIn = true
                if (this.emptyPasswordLogIn)
                    this.passwordIsCorrectLogIn = true

                if(this.emptyPasswordLogIn || this.emptyEmailLogIn)
                    this.componentState = 0
                else
                    this.componentState = 2
            }
        })

    }

    justArrived : boolean = true
    passwordsNotMatch : boolean = false
    emptyPassword : boolean = false
    emptyUserName : boolean = false
    tooLongUserName : boolean = false
    emptyEmail : boolean = false
    emptyRole : boolean = false
    emptyDiscord : boolean = false
    tooLongDiscord : boolean = false
    wrongPicture : boolean = false
    errorHappened : boolean = false
    inputErrors : any = {
        emailIsUnique : true,
        emailIsCorrect : true,
        userNameIsCorrect : true,
        passwordIsCorrect : true
    }
    futureResponse : any = null
    register() {
        this.justArrived = true
        this.tooLongDiscord = false
        this.tooLongUserName = false
        this.passwordsNotMatch = false
        this.emptyPassword = false
        this.emptyUserName = false
        this.emptyEmail = false
        this.emptyRole = false
        this.emptyDiscord = false
        this.wrongPicture = false
        this.errorHappened = false
        this.inputErrors  = {
            emailIsUnique : true,
            emailIsCorrect : true,
            userNameIsCorrect : true,
            passwordIsCorrect : true
        }
        try{
            this.justArrived = false
            this.passwordsNotMatch = false
        
        if(this.roleChosen == ""){
            this.emptyRole = true 
            this.errorHappened = true
        }
        if(this.passwordEnteredRegister == ""){
            this.emptyPassword = true 
            this.errorHappened = true
        }
        if (this.passwordEnteredRegister != this.repeatPasswordRegister){
            this.passwordsNotMatch = true
            this.errorHappened = true
        }
        if(this.userNameEntered == ""){
            this.emptyUserName = true 
            this.errorHappened = true
        }
        if(this.userNameEntered.length > 15)
        {
            this.tooLongUserName = true 
            this.errorHappened = true
        }
        if(this.emailEnteredRegister == ""){
            this.emptyEmail = true 
            this.errorHappened = true
        }
        if(this.discordEntered == ""){
            this.emptyDiscord = true
            this.errorHappened = true
        }
        if(this.discordEntered.length > 20){
            this.tooLongDiscord = true 
            this.errorHappened = true
        }
        if(this.profilePicDisplay == "" ){
            this.wrongPicture = true
            this.errorHappened = true

            this.profilePicDisplay = ""
        }

        let userToPost : UserRegister = {
            userName : this.userNameEntered,
            email : this.emailEnteredRegister,
            password : this.passwordEnteredRegister,
            role : this.roleChosen,
            discord : this.discordEntered,
            profilePicture : this.profilePicDisplay.split(',')[1]
        }
    
        let post = this.http.post(this.urlRegister, userToPost)
        .subscribe({
            next: (response : any) => { 
                this.futureResponse = response
                if (response != null)
                {
                    this.inputErrors.emailIsUnique = response.emailIsUnique
                    this.inputErrors.emailIsCorrect = response.emailIsCorrect
                    this.inputErrors.userNameIsCorrect = response.userNameIsCorrect
                    this.inputErrors.passwordIsCorrect = response.passwordIsCorrect
                }
            },
            complete: () => {
                if(this.futureResponse == null){
                    this.emailEnteredLogIn = this.emailEnteredRegister
                    this.passwordEnteredLogIn = this.passwordEnteredRegister
                    this.logIn(true)
                    this.componentState = 5
                }
                else {this.componentState = 1}
            },
            error: () => {this.componentState = 4}
        })
    }
    catch {
this.componentState = 4
    }
    }

    goToRegister() {
        this.componentState = 1
    }

    cancel() {
        this.componentState = 0
    }

    showImage: boolean = false
    onFileChanged(event : any) {
        const files = event.target.files;
        if (files.length === 0)
            return;
    
    
        const reader = new FileReader();
        this.profilePic = files;
        reader.readAsDataURL(files[0]); 
        reader.onload = (_event) => { 
            this.profilePicDisplay = reader.result; 
        }
        this.showImage = true
    }
    ngOnInit(): void {
    }

    ngOnDestroy(): void {
        
    }
}