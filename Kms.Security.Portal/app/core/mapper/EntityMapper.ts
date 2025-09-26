module nodak.models {
    interface Predicate<T> {
        (item: T): any;
    }

    interface ParamConstructor<T> {
        new (object?): T;
    }

    interface KeyPairValueOnPropNameAndCtor<Ctor> {//Navigiation Property Name and Conسtructor Name
        NavName: string;
        Ctor: ParamConstructor<Ctor>;
        IsCollection: boolean;
    }

    export abstract class EntityMapper<Entity, Dto> implements IEntityMapper<Entity, Dto> {

        constructor() {
        }

        abstract MapToDto(entity: Entity): Dto;
        abstract MapToEntity(dto: Dto): Entity;
    }

}
