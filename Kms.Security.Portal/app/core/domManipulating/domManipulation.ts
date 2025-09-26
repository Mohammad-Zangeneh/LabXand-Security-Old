module nodak {
    export class SetMessageIntoDOM {
        currentElement: ng.IAugmentedJQuery;
        message: string;
        messageType: nodak.enums.MessageType;

        constructor(Id: string, Message: string, messageType: nodak.enums.MessageType) {
            this.currentElement = angular.element(document.getElementById(Id));
            this.message = Message;
            this.messageType = messageType;
        }

        Set(): void {

        }
    }

    export class SetTitle {
        currentElement: ng.IAugmentedJQuery;
        title: string;
        id: string;

        constructor(PropertyName: string, Title: string) {
            this.id = PropertyName + '_Id';
            this.currentElement = angular.element(document.getElementById(this.id));
            //this.currentElement.appendTo('<div>' + this.title + '</div>');
        }
    }

    export class DOMManipulating {

        constructor() { };
        static DisableScreen() {
            document.getElementById('ModalFadeIn').style.display = "block";

        }
        static EnableScreen() {
            document.getElementById('ModalFadeIn').style.display = "none";

        }

        static CreateId(subSystem: nodak.enums.SubSystems, entityName: string, propertyName: string) {
            return subSystem + "_" + entityName + "_" + propertyName + "_Id";
        }

        static CreateIdForSummary(subSystem: nodak.enums.SubSystems, entityName: string, messageType: nodak.enums.MessageType) {
            return subSystem + entityName + messageType + "_Summary";
        }

        static GetClassOfMessageType(messageType: nodak.enums.MessageType) {
            let className = "";

            switch (messageType) {
                case nodak.enums.MessageType.Error:
                    className = "error";
                    break;
                case nodak.enums.MessageType.Information:
                    className = "info";
                    break;
                case nodak.enums.MessageType.Success:
                    className = "suc";
                    break;
                case nodak.enums.MessageType.Warning:
                    className = "warning";
                    break;
            }

            return className;
        }

        //angular is crashed when call this
        static Prepend(Message: string, messageType: nodak.enums.MessageType, NewId?: string): void {
            //this.Message = Message;
            //this.NewId = NewId;
            //this.CurrentElement.parent().parent().find('label').first().append(this.CreatElementByType(MessageType));
        }

        //Inline message for summary
        static MessageTagBuilder(message: nodak.models.Message, entityName: string, subSystem: nodak.enums.SubSystems) {
            let content = `<span id='` + this.CreateIdForSummary(subSystem, entityName, message.messageType) + `' >` + message.text + "</span>";
            return content;
        }

        static RemoveSummarySection(messageType: nodak.enums.MessageType) {
            let id = "";
            document.getElementById(id).remove();
        }

        //key content 
        //Value Id
        static CreateSummarySection(messageType: nodak.enums.MessageType): nodak.models.KeyValuePair<string, string> {
            let summary: nodak.models.KeyValuePair<string, string> = new nodak.models.KeyValuePair<string, string>();

            switch (messageType) {
                case nodak.enums.MessageType.Warning:
                    summary.Key = `<div id="Summary_Warning" class='` + this.GetClassOfMessageType(messageType) + `'> </div>`;
                    summary.Value = "Summary_Warning";
                    break;
                case nodak.enums.MessageType.Error:
                    summary.Key = `<div id="Summary_Error" class='` + this.GetClassOfMessageType(messageType) + `'> </div>`;
                    summary.Value = 'Summary_Error';
                    break;
                case nodak.enums.MessageType.Success:
                    summary.Key = `<div id="Summary_Success" class='` + this.GetClassOfMessageType(messageType) + `'> </div>`;
                    summary.Value = 'Summary_Success';
                    break;
                case nodak.enums.MessageType.Information:
                    summary.Key = `<div  id="Summary_Information" class='` + this.GetClassOfMessageType(messageType) + `'> </div>`;
                    summary.Value = 'Summary_Information';
                    break;
            }
            return summary;
        }

        static CreateIcon(message: nodak.models.Message, entityName: string, subSystem: nodak.enums.SubSystems): nodak.models.KeyValuePair<string, string> {
            let icon: nodak.models.KeyValuePair<string, string> = new nodak.models.KeyValuePair<string, string>()
            icon.Value = this.CreateId(subSystem, entityName, message.propertyName);

            switch (message.messageType) {
                case nodak.enums.MessageType.Warning:
                    icon.Key = `titleIcon`;

                    break;
                case nodak.enums.MessageType.Error:
                    icon.Key = `
<div id='` + icon.Value + "Icon" + `' class="labxand-error">
                    <div class="labxand-error-left"></div>
                    <div class="labxand-error-center">
                    ` + message.text + `
                    </div>
                    <div class="labxand-error-right"></div>
                </div>
`;
                    break;
                case nodak.enums.MessageType.Success:
                    icon.Key = `titleIcon`;
                    break;
                case nodak.enums.MessageType.Information:
                    icon.Key = `titleIcon`;
                    break;
            }
            return icon;
        }

        static Append(content: string, id: string): void {
            let tag = angular.element(document.getElementById(id));
            tag.parent().append(content);
        }

        static RemoveAllErrorIcon() {
            let classNam = "labxand-error";
            let doc = angular.element(document.getElementsByClassName(classNam)).remove();
        }

        static RemoveIcon(propertyName: string, messageType: nodak.enums.MessageType, entityName: string, subSystem: nodak.enums.SubSystems): void {
            let id = this.CreateId(subSystem, entityName, propertyName) + "Icon";
            //console.log(id);
            let doc = document.getElementById(id);
            //console.log(doc);
            if (doc != null)
                doc.remove();
            //.remove();
            //$("div #" + id).remove();
        }
    }
}