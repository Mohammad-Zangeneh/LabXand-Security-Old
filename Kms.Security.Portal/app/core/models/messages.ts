module nodak.models {
    export class Message {
        masterValidationType: nodak.enums.MasterValidationType;
        detailedValidationType: nodak.enums.DetailedValidationType;
        messageType: nodak.enums.MessageType;
        text: string;
        propertyName: string;
        propertyValue: string;
        valueChanged: boolean;
        subSystem: nodak.enums.SubSystems;
        entityName: string;
    }

    export class MessageHandler {
        private messages: Array<Message>;

        //Count of messages
        set CountOfMessage(value: number) { }
        get CountOfMessage(): number { return this.messages.length; }

        constructor() {
            this.messages = new Array<Message>();
        }

        AddMessageForProperty<TResult>(Property: TResult, detailedValidationType: nodak.enums.DetailedValidationType, messageType: nodak.enums.MessageType, text: string) {
            let message = new Message();

            message.masterValidationType = nodak.enums.MasterValidationType.BussinessModelValidation;
            message.detailedValidationType = detailedValidationType;
            message.messageType = messageType;
            message.text = text;
            message.propertyName = nodak.PropertyManipulating.GetName(Property);

            this.messages.push(message);
        }

        ClearErrorMessage() {
            this.messages = this.messages.filter(t => t.messageType != nodak.enums.MessageType.Error);
        }

        ClearMessages() {
            this.messages = new Array<Message>();
        }

        AddMessage(message: Message) {
            this.messages.push(message);
        }

        GetErrorMessages(): Array<Message> {

            return this.messages.filter(t => t.messageType == nodak.enums.MessageType.Error);
        }

        GetWarningMessages(): Array<Message> {
            return this.messages.filter(t => t.messageType == nodak.enums.MessageType.Warning);
        }

        GetInformationMessages(): Array<Message> {
            return this.messages.filter(t => t.messageType == nodak.enums.MessageType.Information);
        }

        GetSuccessMessages(): Array<Message> {
            return this.messages.filter(t => t.messageType == nodak.enums.MessageType.Success);
        }

        GetErrorMessagesByProperty<TResult>(Property: TResult): Array<Message> {
            return this.messages.filter(t => t.messageType == nodak.enums.MessageType.Error &&
                t.propertyName == nodak.PropertyManipulating.GetName(Property));
        }

        RemoveErrorMessagesByProperty(propertyName: string) {
            this.messages = this.messages.filter(q => q.propertyName != propertyName);
        }

        GetInformationMessagesByProperty<TResult>(Property: TResult): Array<Message> {
            return this.messages.filter(t => t.messageType == nodak.enums.MessageType.Information &&
                t.propertyName == nodak.PropertyManipulating.GetName(Property));
        }

        RemoveMessages(messageList: Array<Message>) {
            messageList.forEach((item) => {
                let index = this.messages.indexOf(item);
                this.messages.splice(index);
            });
        }
    }

}