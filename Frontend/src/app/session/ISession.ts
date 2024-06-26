import { GmForSession } from "../gm/GmForSession"
import { PlayerForSession } from "../player/PlayerForSession"

export default interface ISession {
    system : string
    title : string
    tags : string
    description : string
    maxNumberOfPlayers : number
    gm: GmForSession
    players : PlayerForSession[]

    readonly currentNumberOfPlayers : number
}