import { empty } from "rxjs"
import { PlayerForSession } from "../player/PlayerForSession"
import { GmForSession } from "../gm/GmForSession"

export class Session
{
    system: string = ""
    title: string = ""
    tags: string = ""
    description: string = ""
    gm! : GmForSession
    players! : PlayerForSession[]
    currentNumberOfPlayers: number = 0
    maxNumberOfPlayers: number = 0
}