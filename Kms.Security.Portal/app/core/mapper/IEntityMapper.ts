module nodak.models {
    export interface IEntityMapper<Entity, Dto> {
        MapToDto(entity: Entity): Dto;
        MapToEntity(dto: Dto): Entity;
    }
}

