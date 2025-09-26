module nodak.validating {
    export class Mask {
        constructor(private ListValidation: Array<IValidationRule>, private ProptertyName: string) {

        }
        IsPositiveNumber() {
            this.ListValidation.push(new PositiveNumberValidation(this.ProptertyName));
            return this;
        }

        IsNumberWithDash() {
            this.ListValidation.push(new NumberWithDashValidation(this.ProptertyName));
            return this;
        }

        IsFloatPositiveNumber() {
            this.ListValidation.push(new FloatPositiveNumberValidation(this.ProptertyName));
            return this;
        }

        IsFloatNegativeNumber() {
            this.ListValidation.push(new FloatNegativeNumberValidation(this.ProptertyName));
            return this;
        }

        IsEnglishLetters() {
            this.ListValidation.push(new EnglishLettersValidation(this.ProptertyName));
            return this;
        }

        IsPersianLetters() {
            this.ListValidation.push(new PersianLettersValidation(this.ProptertyName));
            return this;
        }

        IsNotPersianLetter() {
            this.ListValidation.push(new NotPersianLettersValidation(this.ProptertyName));
            return this;
        }

        IsNumberWithEnglishLetter() {
            this.ListValidation.push(new NumberWithEnglishLetterValidation(this.ProptertyName));
            return this;
        }
        IsNumberWithEnglishLetterAndDashORUnderScore() {
            this.ListValidation.push(new NumberWithEnglishLetterWithDashOrUnderScoreValidation(this.ProptertyName));
            return this;
        }

        IsNumberWithPersianLetter() {
            this.ListValidation.push(new NumberWithPersianLetterValidation(this.ProptertyName));
            return this;
        }

        IsValidNationalityCode() {
            this.ListValidation.push(new CheckNationalityCode(this.ProptertyName));
            return this;
        }
    }
}