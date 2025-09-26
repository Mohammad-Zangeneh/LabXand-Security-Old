module nodak.validating {

    export class RegularExpersion {

        CheckNationalityCode(NationalityCode: string) {
            var meli_code;
            //if (NationalityCode == undefined)
            //    return false;
            meli_code = NationalityCode;
            if (meli_code.length == 10) {
                if (meli_code == "1111111111" ||
                    meli_code == "0000000000" ||
                    meli_code == "2222222222" ||
                    meli_code == "3333333333" ||
                    meli_code == "4444444444" ||
                    meli_code == "5555555555" ||
                    meli_code == "6666666666" ||
                    meli_code == "7777777777" ||
                    meli_code == "8888888888" ||
                    meli_code == "9999999999") {

                    return false;
                }
                var c = parseInt(meli_code.charAt(9));
                var n = parseInt(meli_code.charAt(0)) * 10 +
                    parseInt(meli_code.charAt(1)) * 9 +
                    parseInt(meli_code.charAt(2)) * 8 +
                    parseInt(meli_code.charAt(3)) * 7 +
                    parseInt(meli_code.charAt(4)) * 6 +
                    parseInt(meli_code.charAt(5)) * 5 +
                    parseInt(meli_code.charAt(6)) * 4 +
                    parseInt(meli_code.charAt(7)) * 3 +
                    parseInt(meli_code.charAt(8)) * 2;
                var r = n - Math.floor(n / 11) * 11;
                if ((r == 0 && r == c) || (r == 1 && c == 1) || (r > 1 && c == 11 - r)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        IsPositiveNumber(model: number) {
            if (model == undefined)
                return model;
            else
                return +(model.toString().replace(/[^0-9]+/g, ''));
        }

        IsNumberWithDash(model: number) {
            if (model == null)
                return model;
            else {
                let value = +(model.toString().replace(/[^-\d]/, '').toString());
                if (isNaN(value))
                    value = 0;

                return value;
            }
        }

        IsFloatPositiveNumber(model: string) {
            if (model == undefined)
                return model;
            else
                return model.replace(/[^\d\.\d]/, '');
        }

        IsFloatNegativeNumber(model: string) {
            return model.replace(/[^-\d\.\d]/, '');
        }

        IsPersianLetters(model: string) {
            return model.replace(/[^\u0600-\u06FF\uFB8A\u067E\u0686\u06AF]+$/, '');
        }

        IsEnglishLetters(model: string) {
            return model.replace(/[^\a-zA-Z]*$/, '');
        }

        IsNotPersianLetter(model: string) {//Do Not Type Persian, Other keys Enable (for example:URL)
            return model.replace(/[\u0600-\u06FF\uFB8A\u067E\u0686\u06AF]+$/, '');
        }

        IsNumberWithEnglishLetter(model: string) {
            return model.replace(/[^a-zA-Z0-9]+$/i, '');
        }

        IsNumberWithEnglishLetterWithDashOrUnderScore(model: string) {
            if (model == undefined)
                return model;
            else
                return model.replace(/[^a-zA-Z0-9-_]+$/i, '');
        }

        IsNumberWithPersianLetter(model: string) {
            return model.replace(/[^\u0600-\u06FF\uFB8A\u067E\u0686\u06AF0-9]+$/, '');
        }

        checkNationalityCode = function (NationalityCode) {
            var meli_code;
            if (NationalityCode == undefined)
                return false;
            meli_code = NationalityCode;
            if (meli_code.length == 10) {
                if (meli_code == "1111111111" ||
                    meli_code == "0000000000" ||
                    meli_code == "2222222222" ||
                    meli_code == "3333333333" ||
                    meli_code == "4444444444" ||
                    meli_code == "5555555555" ||
                    meli_code == "6666666666" ||
                    meli_code == "7777777777" ||
                    meli_code == "8888888888" ||
                    meli_code == "9999999999") {

                    return false;
                }
                var c = parseInt(meli_code.charAt(9));
                var n = parseInt(meli_code.charAt(0)) * 10 +
                    parseInt(meli_code.charAt(1)) * 9 +
                    parseInt(meli_code.charAt(2)) * 8 +
                    parseInt(meli_code.charAt(3)) * 7 +
                    parseInt(meli_code.charAt(4)) * 6 +
                    parseInt(meli_code.charAt(5)) * 5 +
                    parseInt(meli_code.charAt(6)) * 4 +
                    parseInt(meli_code.charAt(7)) * 3 +
                    parseInt(meli_code.charAt(8)) * 2;
                var r = n - Math.floor(n / 11) * 11;
                if ((r == 0 && r == c) || (r == 1 && c == 1) || (r > 1 && c == 11 - r)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        };
    }
} 