import { GameSystem } from "./GameSystem"
import { GmForSession } from "./GmForSession"

export default interface ISessionPost {
    system : GameSystem
    title : string
    tags : string
    triggers: string
    vtt: string
    location : string
    date : string | undefined
    description : string
    maxNumberOfPlayers : number
    picture: string
    gmUserName : string
    isRemote : boolean
}