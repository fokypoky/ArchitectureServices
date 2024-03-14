CREATE table courses (
	id serial not null PRIMARY KEY,
	title VARCHAR(300) NOT NULL UNIQUE,
	duration int,
	description_id VARCHAR(50)
);

CREATE TABLE specialities (
	id serial not null PRIMARY KEY,
	title varchar(200) not null,
	code varchar(50) not null UNIQUE,
	study_duration int
);

CREATE TABLE departments (
	id serial not null PRIMARY KEY,
	title VARCHAR(200) NOT NULL UNIQUE,
	institute_id INT,
	main_speciality_id int not null,
	FOREIGN KEY(main_speciality_id) REFERENCES specialities(id)
);

create table groups (
	id serial not null PRIMARY KEY,
	number varchar(50) UNIQUE,
	department_id int not null,
	speciality_id int not null,
	FOREIGN KEY(department_id) references departments(id),
	FOREIGN KEY(speciality_id) REFERENCES specialities(id)
);

CREATE table lecture_types (
	id serial not null PRIMARY KEY,
	type varchar(50) unique
);

create table lectures (
	id serial not null PRIMARY KEY,
	annotation varchar(200),
	type_id int not null,
	course_id int not null,
	requirements varchar(350),
	FOREIGN KEY(type_id) REFERENCES lecture_types(id),
	FOREIGN KEY(course_id) REFERENCES courses(id)
);

CREATE TABLE lecture_materials (
	id serial not null PRIMARY KEY,
	lecture_id int not null,
	materials_id varchar(200),
	FOREIGN KEY(lecture_id) REFERENCES lectures(id)
);

CREATE TABLE students (
	id serial not null PRIMARY KEY,
	name varchar(200) not null,
	group_id int not null,
	passbook_number varchar(50) not null UNIQUE,
	foreign key(group_id) REFERENCES groups(id)
);

CREATE TABLE timetable (
	id serial not null PRIMARY KEY,
	group_id int not null,
	lecture_id int not null,
	date timestamp not null,
	FOREIGN KEY(group_id) REFERENCES groups(id),
	FOREIGN KEY(lecture_id) REFERENCES lectures(id)
);

create table visits (
	student_id int not null,
	lecture_id int not null,
	date timestamp not null,
	FOREIGN KEY(student_id) REFERENCES students(id),
	FOREIGN KEY(lecture_id) REFERENCES lectures(id)
) PARTITION BY RANGE(date);

CREATE TABLE visits_202305 PARTITION of visits
	FOR VALUES FROM ('2023-05-01T00:00:00') TO ('2023-05-31T23:59:59');

CREATE TABLE visits_202306 PARTITION of visits
	FOR VALUES FROM ('2023-06-01T00:00:00') TO ('2023-06-30T23:59:59');

CREATE TABLE visits_202307 PARTITION of visits
	FOR VALUES FROM ('2023-07-01T00:00:00') TO ('2023-07-31T23:59:59');