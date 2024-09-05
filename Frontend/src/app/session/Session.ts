import { PlayerForSession } from "../player/PlayerForSession"
import { GmForSession } from "../gm/GmForSession"
import { GameSystem } from "../gameSystem/GameSystemModel"

export class Session
{
    system!: GameSystem
    title: string = ""
    tags: string = ""
    description: string = ""
    gm : GmForSession = new GmForSession()
    players! : PlayerForSession[]
    currentNumberOfPlayers: number = 0
    maxNumberOfPlayers: number = 0
}