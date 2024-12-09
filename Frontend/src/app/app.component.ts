import { Component, EventEmitter, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from "@angular/core"
import { UserService } from "./services/user.service"
import { Subscription } from "rxjs"

@Component({
    selector: "app",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.css"],
})
export class App implements OnInit, OnDestroy, OnChanges
{
    constructor(private userService : UserService) {}
    profilePicSrc : string = ""
    userName : string = ""
    profilePicPlaceHolder : string = "/assets/profilepic.png"
    userNamePlaceHolder : string = "<UserName>"
    currentUserRole : string = ""

    isMenuOpened : boolean = false
    toggleMenu() : void
    {
        this.isMenuOpened = !this.isMenuOpened
    }
    clickedOutside() : void
    {
        this.isMenuOpened = false
    }

    showSessionDetails : boolean = false
    onSessionClicked(){
        this.showSessionDetails = true
    }

    burgerClicked : boolean = false
    burgerNotClicked : boolean = true
    onBurgerClicked() {
        this.burgerClicked = !this.burgerClicked
        this.burgerNotClicked = !this.burgerNotClicked
    }

    chosenExplore : boolean = false
    chosenCreateSession : boolean = false
    chosenMySessions : boolean = false
    chosenAccount : boolean = false
    chosenLoginRegister : boolean = true
    chosenSessionParameters : boolean = false
    chosenUsers : boolean = false

    onExploreClicked() {
        this.chosenMySessions = false
        this.chosenAccount = false
        this.chosenCreateSession = false
        this.chosenLoginRegister = false
        this.chosenUsers = false
        this.chosenSessionParameters = false
        this.chosenExplore = true
    }

    onMySessionsClicked() {
        this.chosenExplore = false
        this.chosenAccount = false
        this.chosenCreateSession = false
        this.chosenLoginRegister = false
        this.chosenUsers = false
        this.chosenSessionParameters = false
        this.chosenMySessions = true
    }

    onAccountClicked() {
        this.chosenExplore = false
        this.chosenMySessions = false
        this.chosenCreateSession = false
        this.chosenLoginRegister = false
        this.chosenUsers = false
        this.chosenSessionParameters = false
        this.chosenAccount = true
    }

    onCreateSessionClicked() {
        this.chosenMySessions = false
        this.chosenAccount = false
        this.chosenExplore = false
        this.chosenLoginRegister = false
        this.chosenUsers = false
        this.chosenSessionParameters = false
        this.chosenCreateSession = true
    }

    onUsersClicked() {
        this.chosenMySessions = false
        this.chosenAccount = false
        this.chosenCreateSession = false
        this.chosenExplore = false
        this.chosenLoginRegister = false
        this.chosenSessionParameters = false
        this.chosenUsers = true
    }

    onSessionParametersClicked() {
        this.chosenMySessions = false
        this.chosenAccount = false
        this.chosenCreateSession = false
        this.chosenExplore = false
        this.chosenLoginRegister = false
        this.chosenUsers = false
        this.chosenSessionParameters = true
    }

    onHomeClicked() {
        this.chosenMySessions = false
        this.chosenAccount = false
        this.chosenExplore = false
        this.chosenCreateSession = false
        this.chosenUsers = false
        this.chosenSessionParameters = false
        this.chosenLoginRegister = true
    }

    userSub! : Subscription
    ngOnInit(): void {
        this.userSub = this.userService.currentUser$.subscribe(() => {
            if(this.userService.userIsLoggedIn){
                var currentUser = this.userService.getCurrentUser()
                this.profilePicSrc = currentUser.profilePicture
                this.userName = currentUser.userName
                this.currentUserRole = currentUser.role
            }
            else {
                this.profilePicSrc = this.profilePicPlaceHolder
                this.userName = this.userNamePlaceHolder
                this.currentUserRole = "Guest"
            }
        })
    }
    ngOnDestroy(): void {
        this.userSub.unsubscribe()
    }

    ngOnChanges(changes: SimpleChanges): void {
        throw new Error("Method not implemented.")
    }
}