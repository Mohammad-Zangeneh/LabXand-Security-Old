module nodak.validating {
    export class ValidationModel {
        modelList: Array<string>;

        constructor() {
            this.modelList = new Array<string>();

        }

        Include<T>(name: T) {
            this.modelList.push("this." + nodak.PropertyManipulating.GetName(name));
            return this;
        }

        Clear() {
            this.modelList = new Array<string>();
        }
    }

    export class ImportantPropertyForModel {
        propetyNameList: Array<string>;

        constructor() {
            this.propetyNameList = new Array<string>();

        }

        Include<T>(name: T) {
            this.propetyNameList.push("this." + nodak.PropertyManipulating.GetName(name));
            return this;
        }

        Clear() {
            this.propetyNameList = new Array<string>();
        }
    }

    export interface IValidationRule {
        CheckValidation(value: any, PropertyName: string): nodak.models.Message;
    }

    export abstract class ValidationBase implements IValidationRule {
        message: nodak.models.Message;

        constructor(detailedValidationType: enums.DetailedValidationType, propertyName: string, valueChanged: boolean) {
            this.message = new nodak.models.Message();
            this.message.masterValidationType = enums.MasterValidationType.Guard;
            this.message.detailedValidationType = detailedValidationType;
            this.message.valueChanged = valueChanged;
            this.message.propertyName = propertyName;
            this.message.text = null;
        }
        abstract CheckValidation(value: any, title: string): nodak.models.Message;
    }
    //Expersions
    export class CheckNationalityCode extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.CheckNationalityCode, propertyName, false)
        }

        CheckValidation(value: string, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;

            var result = new RegularExpersion().CheckNationalityCode(value);
            if (result == false) {
                if (value != "")
                    this.message.text = "<span>کد ملی وارد شده معتبر نمی باشد.</span>";
            }

            return this.message;
        }
    }

    export class PositiveNumberValidation extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.PositiveNum, propertyName, true)
        }

        CheckValidation(value: string, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;
            this.message.propertyValue = new RegularExpersion().IsPositiveNumber(+value).toString();

            return this.message;
        }
    }

    export class NumberWithDashValidation extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.NumberWithDash, propertyName, true)
        }
        CheckValidation(value: string, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;

            this.message.propertyValue = new RegularExpersion().IsNumberWithDash(+value).toString();

            return this.message;
        }
    }

    export class FloatPositiveNumberValidation extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.FloatPositiveNumber, propertyName, true)
        }

        CheckValidation(value: string, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;

            this.message.propertyValue = new RegularExpersion().IsFloatPositiveNumber(value);

            return this.message;
        }
    }

    export class FloatNegativeNumberValidation extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.FloatNegativeNumber, propertyName, true)
        }

        CheckValidation(value: string, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;
            this.message.propertyValue = new RegularExpersion().IsFloatNegativeNumber(value);

            return this.message;
        }
    }

    export class EnglishLettersValidation extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.EnglishLetter, propertyName, true)
        }

        CheckValidation(value: string, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;
            if (value != null)
                this.message.propertyValue = new RegularExpersion().IsEnglishLetters(value);

            return this.message;
        }
    }

    export class PersianLettersValidation extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.PersianLetters, propertyName, true)
        }

        CheckValidation(value: string, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;

            this.message.propertyValue = new RegularExpersion().IsPersianLetters(value);

            return this.message;
        }
    }

    export class NotPersianLettersValidation extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.NotPersianLetter, propertyName, true)
        }

        CheckValidation(value: string, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;

            this.message.propertyValue = new RegularExpersion().IsNotPersianLetter(value);

            return this.message;
        }
    }


    export class NumberWithEnglishLetterValidation extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.NumberWithEnglishLetter, propertyName, true)
        }

        CheckValidation(value: string, PropertyName: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;
            this.message.propertyValue = new RegularExpersion().IsNumberWithEnglishLetter(value);

            return this.message;
        }
    }

    export class NumberWithEnglishLetterWithDashOrUnderScoreValidation extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.NumberWithEnglishLetterWithDashOrUnderScore, propertyName, true)
        }

        CheckValidation(value: string, PropertyName: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;
            this.message.propertyValue = new RegularExpersion().IsNumberWithEnglishLetterWithDashOrUnderScore(value);

            return this.message;
        }
    }

    export class NumberWithPersianLetterValidation extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.NumberWithPersianLetter, propertyName, true)
        }

        CheckValidation(value: string, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;
            this.message.propertyValue = new RegularExpersion().IsNumberWithPersianLetter(value);

            return this.message;
        }
    }

    export class MinLengthValidation extends ValidationBase {
        minValueChecked: number;
        constructor(MinValue: number, propertyName: string) {
            super(enums.DetailedValidationType.MinLength, propertyName, false);
            this.minValueChecked = MinValue;
        }

        CheckValidation(Value: any, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            var stringValue: string = Value;
            this.message.text = null;

            if (stringValue == undefined && stringValue == "" && stringValue == null)
                return null;

            if (stringValue.trim() != "") {
                if (stringValue.length < this.minValueChecked) {
                    this.message.text = "<span>تعداد حروف \" " + title + "\" باید بزرگتر از " + this.minValueChecked + " باشد.</span>";
                    return this.message;
                }
            }

            return this.message;
        }
    }

    export class RequiredValidation extends ValidationBase {

        constructor(propertyName: string) {
            super(enums.DetailedValidationType.Required, propertyName, false);
        }

        CheckValidation(Value: string, title: string): nodak.models.Message {

            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;

            if (Value == null || Value == undefined || Value == "")
                this.message.text = "<span>وارد نمودن \" " + title + "\" ضروری است.</span>";

            return this.message;
        }

    }
    export class EmailValidation extends ValidationBase {

        constructor(propertyName: string) {
            super(enums.DetailedValidationType.Required, propertyName, false);
        }
        validateEmail(email) {
            if (email == null || email == undefined || email == "")
                return true;
            var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        }
        CheckValidation(Value: string, title: string): nodak.models.Message {

            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;
            if (!this.validateEmail(Value))
                this.message.text = "<span>مقدار \" " + title + "\" مقدار معتبری برای ایمیل نیست.</span>";

            return this.message;
        }

    }
    export class MaxLengthValidation extends ValidationBase {
        maxValueChecked: number;
        constructor(MaxValue: number, propertyName: string) {
            super(enums.DetailedValidationType.MaxLength, propertyName, false);
            this.maxValueChecked = MaxValue;
        }

        CheckValidation(Value: any, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;
            this.message.text = null;
            var stringValue: string = Value;
            if (stringValue == undefined && stringValue == null)
                return null;

            if (stringValue.length > this.maxValueChecked) {
                this.message.text = "<span>تعداد حروف \" " + title + "\" باید کوچکتر از " + this.maxValueChecked + " باشد.</span>";
                return this.message;
            }

            return this.message;
        }
    }

    export class SpecificValidation extends ValidationBase {
        constructor(detailedValidationType: enums.DetailedValidationType,
            propertyName: string,
            valueChanged: boolean) {
            super(detailedValidationType, propertyName, false);
        }

        CheckValidation(Value: any, title: string): nodak.models.Message {

            return this.message;
        }
    }
    //======================================================
    class LessThanNowValidation extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.LessThanNow, propertyName, true)
        }
        CheckValidation(value: string, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;

            if (!LessThanNow(value)) {
                this.message.text = "مقدار تاریخ باید کوچکتر از تاریخ جاری باشد.";
                return this.message;
            }
            else
                this.message.text = null;

            return this.message;
        }
    }

    class GratherThanNowValidation extends ValidationBase {
        constructor(propertyName: string) {
            super(enums.DetailedValidationType.LessThanNow, propertyName, true)
        }

        CheckValidation(value: string, title: string): nodak.models.Message {
            this.message.messageType = nodak.enums.MessageType.Error;

            if (!GratherThanNow(value)) {
                this.message.text = "مقدار تاریخ باید بزرگتر از تاریخ جاری باشد.";
                return this.message;
            }
            else
                this.message.text = null;

            return this.message;
        }
    }

    //==============================================================
    export class PropertySpecification {
        ProptertyName: string;
        Title: string;
        ListValidation: Array<IValidationRule>;

        constructor(propertyName: string, title: string) {
            this.ListValidation = [];
            this.ProptertyName = propertyName;
            this.Title = title;
        }

        HasSpecificRole(func: Function) {
            let specific = new SpecificValidation(null, null, null);
            let propName = this.ProptertyName;
            specific.CheckValidation = (value, title): nodak.models.Message => {
                return func(value, title);
            };

            specific.message.propertyName = this.ProptertyName;

            this.ListValidation.push(specific);

            return this;
        }

        HasMaxLength(MaxValue: number) {
            this.ListValidation.push(new MaxLengthValidation(MaxValue, this.ProptertyName));
            return this;
        }

        HasMinLength(MinLengthValue: number) {
            this.ListValidation.push(new MinLengthValidation(MinLengthValue, this.ProptertyName));

            return this;
        }

        IsRequired() {
            this.ListValidation.push(new RequiredValidation(this.ProptertyName));
            return this;
        }
        IsValidEmail() {
            ///EmailValidation
            this.ListValidation.push(new EmailValidation(this.ProptertyName));
            return this;
        }
        IsLessThanNow() {
            this.ListValidation.push(new LessThanNowValidation(this.ProptertyName));
            return this;
        }

        IsGratherThanNow() {
            this.ListValidation.push(new GratherThanNowValidation(this.ProptertyName));
            return this;
        }

        HasMask() {
            let mask = new Mask(this.ListValidation, this.ProptertyName);
            return mask;
        }

    }
}