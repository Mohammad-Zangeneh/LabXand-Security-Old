module nodak.models {
    export class UserCommunicator {

        static $inject = ['messageBox']
        constructor(public messageBox: IMessageBox) {
        }

        private RemoveAndAppendDom(messages: Array<nodak.models.Message>, isShowSummary: boolean, pageType: nodak.enums.PageType, messageType: nodak.enums.MessageType) {
            //Remove Old Icons
            nodak.DOMManipulating.RemoveAllErrorIcon();

            messages.forEach((errorItem) => {
                let icon = nodak.DOMManipulating.CreateIcon(errorItem, errorItem.entityName, errorItem.subSystem);
                nodak.DOMManipulating.Append(icon.Key, icon.Value);
            });

            //Add new message to dom
            if (isShowSummary) {
                let summarySection = nodak.DOMManipulating.CreateSummarySection(messageType);

                switch (pageType) {
                    case nodak.enums.PageType.Page:
                        messages.forEach((errorItem) => {
                            nodak.DOMManipulating.Append(nodak.DOMManipulating.MessageTagBuilder(errorItem, errorItem.entityName, errorItem.subSystem), summarySection.Value);
                        });
                        break;
                }
            }
        }

        RemoveAllErrorIcon() {
            nodak.DOMManipulating.RemoveAllErrorIcon();
        }

        RemoveIconFromProperties(propertyList: Array<nodak.validating.PropertySpecification>,
            messageType: nodak.enums.MessageType,
            entityName: string,
            subSystem: nodak.enums.SubSystems) {

            propertyList.forEach((prop) => {
                nodak.DOMManipulating.RemoveIcon(prop.ProptertyName, messageType, entityName, subSystem);
            })

        }

        ShowErrorMessages(messages: Array<nodak.models.Message>, isShowSummary: boolean, pageType: nodak.enums.PageType) {
            this.RemoveAndAppendDom(messages, isShowSummary, pageType, nodak.enums.MessageType.Error);
        }

        ShowSuccessMessages(messages: Array<nodak.models.Message>, isShowSummary: boolean, pageType: nodak.enums.PageType) {
            this.RemoveAndAppendDom(messages, isShowSummary, pageType, nodak.enums.MessageType.Success);
        }

        ShowInformationMessages(messages: Array<nodak.models.Message>, isShowSummary: boolean, pageType: nodak.enums.PageType) {
            this.RemoveAndAppendDom(messages, isShowSummary, pageType, nodak.enums.MessageType.Information);
        }

        ShowWarningMessages(messages: Array<nodak.models.Message>, isShowSummary: boolean, pageType: nodak.enums.PageType) {
            this.RemoveAndAppendDom(messages, isShowSummary, pageType, nodak.enums.MessageType.Warning);
        }

        ShowMessage(message: string) {
            //not implemented
        }
    }
}