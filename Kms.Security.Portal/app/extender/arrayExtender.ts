
interface Array<T> {
    contains(item: T): boolean;
    removeItem(item: T): T;
}

Array.prototype.contains = (item: any) => {
    for (let i in this) {
        if (this[i] === item) return true;
    }
    return false;
}

Array.prototype.removeItem = function (item) {
    if (item != null) {
        let indexItem = this.indexOf(item);
        return this.splice(indexItem, 1);
    }
    else
        return this;
}
