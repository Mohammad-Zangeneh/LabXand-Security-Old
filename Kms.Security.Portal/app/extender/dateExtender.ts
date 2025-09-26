interface Date {

    /** 
   *Compares thisDate with compare date , if yourDate is greather than compareDate then return true or return false
   * @yourDate  the date which is going to be compared.
   * @CompareDate  the date which will be compared with.
  */
    isGreatherThan(thisDate: Date, CompareDate: Date): boolean;
}

Date.prototype.isGreatherThan = (yourDate: Date, CompareDate: Date) => {
    if (yourDate > CompareDate)
        return true;
    return false;
}