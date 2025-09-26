module nodak.components.controllers {
    class Coordinate {
        Id: number;
        EnglishName: string;
        Type: string;
    }

    export class WidthCombo extends nodak.models.BaseStaticCombo<Coordinate> {
        constructor() {
            
            super();
            this.model = [{ Id: 1, EnglishName: 'West', Type: 'lat' }];
            this.valueMember = 'Id';
            this.displayMember = 'EnglishName';
            this.headers = [{ field: 'EnglishName', htmlAttribute: '', title: '' }];
        }
    }
    export class StaticComboController {
        singleStaticCombo: nodak.components.controllers.WidthCombo;
        constructor() {
            this.singleStaticCombo = new nodak.components.controllers.WidthCombo();
        }
    }
}
componentsControllersModule.controller('test', nodak.components.controllers.StaticComboController);