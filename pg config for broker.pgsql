alter table courses replica identity FULL;
alter table departments replica identity FULL;
alter table groups replica identity FULL;
alter table lecture_materials replica identity FULL;
alter table lecture_types replica identity FULL;
alter table lectures replica identity FULL;
alter table specialities replica identity FULL;
alter table students replica identity FULL;
alter table timetable replica identity FULL;
alter table visits_202305 replica identity FULL;
alter table visits_202306 replica identity FULL;
alter table visits_202307 replica identity FULL;

select pg_create_logical_replication_slot('pg_debezium_slot', 'pgoutput');

CREATE PUBLICATION pub FOR ALL TABLES;