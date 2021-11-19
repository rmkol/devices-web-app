--- USER
drop table if exists devices.token;
drop table if exists devices.user cascade;
create table devices.user
(
    id            int          not null generated always as identity primary key,
    email_address varchar(100) not null,
    password      varchar(256) not null,
    first_name    varchar(100) not null,
    last_name     varchar(100) not null,
    created_ts    timestamp    not null default now(),
    updated_ts    timestamp,
    constraint u_user_email_address unique (email_address)
);

create index "idx_user_email_pwd"
    on devices.user (email_address, password);

truncate table devices.user restart identity cascade;
insert into devices.user(email_address, password, first_name, last_name) -- password: very_strong_password
values ('isw@mail.com', 'ee3f4662adbe84ec5a73d1ee98d4c90eabf0471789581a4cdae11b74cd48594b', 'Isaac', 'Newton');

--- ROLE
drop table if exists devices.role cascade;
create table devices.role
(
    id          int         not null generated always as identity primary key,
    name        varchar(64) not null,
    description varchar(128),
    constraint u_role_name unique (name)
);

create index "idx_role_name"
    on devices.role (name);

truncate table devices.role restart identity cascade;
insert into devices.role(name, description)
values ('device_manager', 'Allows to change state for existing device.');

--- USER_ROLE
drop table if exists devices.user_role;
create table devices.user_role
(
    user_id int not null,
    role_id int not null,
    constraint pkey_user_role primary key (user_id, role_id),
    constraint fkey_user_role_user foreign key (user_id) references devices.user (id) on delete cascade,
    constraint fkey_user_role_role foreign key (role_id) references devices.role (id) on delete cascade
);

truncate table devices.user_role restart identity cascade;
insert into devices.user_role(user_id, role_id)
values ((select id from devices.user where email_address = 'isw@mail.com'),
        (select id from devices.role where name = 'device_manager'));

--- TOKEN
create table devices.token
(
    id         int          not null generated always as identity primary key,
    token      varchar(300) not null,
    user_id    int          not null,
    ttl_sec    bigint       not null,
    created_ts timestamp    not null default now(),
    updated_ts timestamp,
    constraint fkey_token_user foreign key (user_id) references devices.user (id) on delete cascade
);

create index "idx_token_token"
    on devices.token (token);

create index "idx_token_user"
    on devices.token (user_id);

--- DEVICE_TYPE
drop table if exists devices.device_type;
create table devices.device_type
(
    id          int          not null generated always as identity primary key,
    name        varchar(512) not null,
    description varchar(1024),
    constraint u_device_type_name unique (name)
);

truncate table devices.device_type;
insert into devices.device_type(name, description)
values ('ATM', 'ATM machine'),
       ('ATM_TEMP_SENSOR', 'ATM temperature sensor'),
       ('ATM_SCREEN', 'ATM screen'),
       ('S_CAMERA', 'Surveillance camera');

--- DEVICE
drop table if exists devices.device;
create table devices.device
(
    id               int          not null generated always as identity primary key,
    name             varchar(512) not null,
    description      varchar(1024),
    type_id          int          not null,
    status           varchar(32)  not null,
    parent_device_id int,
    created_ts       timestamp    not null default now(),
    updated_ts       timestamp,
    constraint u_device_name unique (name),
    constraint check_device_status check (status = 'active' or status = 'disabled' or status = 'deleted')
);
alter table devices.device
    add constraint fkey_device_device foreign key (parent_device_id) references devices.device (id);

create index "idx_device_name"
    on devices.device (name);

truncate table devices.device;
insert into devices.device(type_id, name, status)
values ((select id from devices.device_type where name = 'S_CAMERA'), 'Camera 01', 'active'),
       ((select id from devices.device_type where name = 'S_CAMERA'), 'Camera 02', 'disabled');
insert into devices.device(type_id, name, status)
values ((select id from devices.device_type where name = 'ATM'), 'ATM 01', 'active');

insert into devices.device(type_id, parent_device_id, name, status)
values ((select id from devices.device_type where name = 'ATM_TEMP_SENSOR'),
        (select id from devices.device pd where pd.name = 'ATM 01'),
        'ATM 01 temperature sensor', 'active');
insert into devices.device(type_id, parent_device_id, name, status)
values ((select id from devices.device_type where name = 'ATM_SCREEN'),
        (select id from devices.device pd where pd.name = 'ATM 01'),
        'ATM 01 screen', 'active');