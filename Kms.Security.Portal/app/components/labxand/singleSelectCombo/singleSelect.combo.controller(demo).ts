//module imas.components.controllers {
//    export class singleSelectComboController {
//        currentModel: imas.communications.models.MessageType;
//        messageTypeCombo: imas.communications.models.MessageTypeCombo;

//        static $inject = ['communications.messageType.service'];
//        constructor(private service: IServiceBase<imas.communications.models.MessageType>) {
//            this.currentModel = new imas.communications.models.MessageType();
//            this.messageTypeCombo = new imas.communications.models.MessageTypeCombo(service);

//            this.messageTypeCombo.AfterChange = () => {
//               console.log('afterChange');
//               // this.messageTypeCombo.comboModel = this.messageTypeCombo.comboModel.filter((model) => { return model.Id.toString() == this.messageTypeCombo.selectedId; })
//            }

//            this.messageTypeCombo.AfterSelect = (id) => {
//                console.log('afterSelect');
//                //this.messageTypeCombo.comboModel = this.messageTypeCombo.comboModel.filter((model) => { return model.Id.toString() == this.messageTypeCombo.selectedId; })
//            }

//            this.messageTypeCombo.BeforSelect = () => {
//                console.log('beforSelect');
//            }
//        }
//    }
//}

//componentsControllersModule.controller('components.singleSelectCombo.controller', imas.components.controllers.singleSelectComboController);
