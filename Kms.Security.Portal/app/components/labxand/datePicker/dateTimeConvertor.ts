function IsValidPersinaDate(persianDate: string) {
    if (this.moment(persianDate, 'jYYYY/jM/jD', true).isValid())
        return true;
    if (this.moment(persianDate, 'jYYYY/jMM/jDD', true).isValid())
        return true;

    return false;
   
}
// count can positive or negative
function AddYear(gregorianDate: string, count: number) {
    return this.moment(gregorianDate, 'YYYY/MM/DD').add(count, 'y').format('YYYY/MM/DD');
}
function AddYearWithTime(gregorianDate: string, count: number) {
    return this.moment(gregorianDate, 'YYYY/MM/DD HH:mm').add(count, 'y').format('YYYY/MM/DD HH:mm');
}
function ToGregorianDateTimeFromDB(gregorianDate: string) {
    return this.moment(gregorianDate).format('YYYY/MM/DD HH:mm:ss');
}

function ToPersianDateTimeFromDB(gregorianDate: string) {
    return this.moment(gregorianDate).format('jYYYY/jMM/jDD HH:mm');
}
function ToPersianDate(gregorianDate: string) {
    return this.moment(gregorianDate).format('jYYYY/jMM/jDD');
}
function IsValidGregorianDate(gregorianDate: string) {
    return this.moment(gregorianDate, 'YYYY/MM/DD', true).isValid();
}

function IsValidPersianDateTime(persinaDateTime: string) {
    return this.moment(persinaDateTime, 'jYYYY/jMM/jDD HH:mm', true).isValid();
}

function IsValidGregorianDateTime(gregorianDateTime: string) {
    return this.moment(gregorianDateTime, 'YYYY/MM/DD HH:mm', true).isValid();
}

function PersianToGregorianDate(persinaDate: string) {
    return this.moment(persinaDate, 'jYYYY/jMM/jDD HH:mm').format('YYYY/MM/DD');
}

function GetDateTimeNowGregorian() {
    return PersianToGregorianDateTime(GetDateTimeNowPersian());
}

function GetDateTimeNowPersian() {
    var date = new Date();
    var now = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
    return GregorianToPersianDate(now);
}

function GregorianToPersianDate(gregorianDate: string) {
    return this.moment(gregorianDate, 'YYYY/MM/DD HH:mm').format('jYYYY/jMM/jDD');
}

function PersianToGregorianDateTime(persinaDate: string) {
    return this.moment(persinaDate, 'jYYYY/jMM/jDD HH:mm').format('YYYY/MM/DD HH:mm');
}

function PersianJustTime(gregorianDate: string) {
    return this.moment(gregorianDate, 'YYYY/MM/DD HH:mm').format('HH:mm');
}

function GregorianToPersianDateTime(gregorianDate: string) {
    return this.moment(gregorianDate, 'YYYY/MM/DD HH:mm').format('jYYYY/jMM/jDD HH:mm');
}

function testInputTime(time: string) {
    var str = time.trim().split(":");
    var houres = parseInt(str[0].slice(-2));
    var Minuts = parseFloat(str[1].slice(-2));
    return (houres < 24 && Minuts <= 59);
}

function LessThanNow(DateTime: string) {
    var nowTime = new Date();
    var newTime = new Date(DateTime);
    //console.log(newTime.getTime());
    //console.log(newTime.getTime() <= nowTime.getTime());

    if (newTime.getTime() <= nowTime.getTime())
        //alert("تاریخ مربوط به گذشته است");
        return true;
    else
        return false;
    //alert("تاریخ مربوط به آینده است");
}

function GratherThanNow(DateTime: string) {
    var nowTime = new Date();
    var newTime = new Date(DateTime);
    //console.log(newTime.getTime());
    //console.log(newTime.getTime() >= nowTime.getTime());

    if (newTime.getTime() >= nowTime.getTime())
        //alert("تاریخ مربوط به آینده است");
        return true;
    else
        return false;
    //alert("تاریخ مربوط به گذشته است");
}