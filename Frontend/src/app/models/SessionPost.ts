import { GameSystem } from "./GameSystem"
import { GmForSession } from "./GmForSession"
import ISessionPost from "./ISessionPost"

export class SessionPost implements ISessionPost{
    triggers: string = ""
    vtt: string = ""
    location: string = ""
    date: string | undefined = ""
    isRemote: boolean = false
    system : GameSystem = new GameSystem()
    title : string = ""
    tags : string = ""
    description : string = ""
    maxNumberOfPlayers : number = 0
    picture: string = ""
    //picture : FormData = new FormData()
    gmUserName : string = ""
}