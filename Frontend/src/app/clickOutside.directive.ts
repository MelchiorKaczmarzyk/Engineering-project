import { DOCUMENT } from "@angular/common";
import { AfterViewInit, Directive, ElementRef, EventEmitter, Inject, OnDestroy, Output, inject } from "@angular/core";
import { Subscribable, Subscription, filter, fromEvent } from "rxjs";

@Directive({
    selector: '[clickOutside]'
})
export class ClickedOutsideDirective implements AfterViewInit, OnDestroy
{
    @Output() clickOutside = new EventEmitter<void>()
    constructor(private element : ElementRef, @Inject(DOCUMENT) private document : Document) {}

    documentClickSubscribtion: Subscription | undefined
    ngAfterViewInit(): void 
    {
        this.documentClickSubscribtion = fromEvent(this.document, 'click').pipe(
            filter((event) => {
            return !this.isInside(event?.target as HTMLElement)
            })
        ).subscribe(() => {
            this.clickOutside.emit();
        })
    }

    ngOnDestroy(): void {
        this.documentClickSubscribtion?.unsubscribe
    }
    isInside(elementToCheck : HTMLElement) : boolean {
        return elementToCheck === this.element.nativeElement 
            || this.element.nativeElement.contains(elementToCheck)
    }
}