const { Kafka } = require("kafkajs");
const { ObjectId } = require("mongodb");
require("dotenv").config();

const mongoClient = require("mongodb").MongoClient;

const kafka = new Kafka({
	clientId: "logging-service-client",
	brokers: [`${process.env.BROKER_HOST}:${process.env.BROKER_PORT}`],
});

const consumer = kafka.consumer({ groupId: "data-transform-group" });

const run = async () => {
	const mongo = await mongoClient.connect("mongodb://localhost:27017");
	const db = mongo.db("UniversityDB");
	const collection = db.collection("university");

	await consumer.connect();
	await consumer.subscribe({
		topics: ["pg.public.departments"],
		fromBeginning: false,
	});

	await consumer.run({
		eachMessage: async ({ message }) => {
			try {
				const json = JSON.parse(message.value.toString()).payload;
				console.log("before: ", json.before);
				console.log("after", json.after);

				const document = await collection.findOne({
					_id: new ObjectId("65eb241ce29a7eb4205f6f26"),
				});

				// remove
				if (!json.after) {
					document.institutes.forEach(institute => {
						const searchResult = institute.departments.find(
							d => d.department_name === json.before.title
						);

						if (searchResult) {
							institute.departments.splice(
								institute.departments.indexOf(searchResult)[0],
								1
							);
						}
					});

					collection.updateOne(
						{
							_id: new ObjectId("65eb241ce29a7eb4205f6f26"),
						},
						{ $set: document }
					);

					return;
				}
				// update
				if (json.before && json.after) {
					document.institutes.forEach(institute => {
						const searchResult = institute.departments.some(
							d => d.department_name === json.before.title
						);

						if (searchResult) {
							const newDepartment = {
								department_name: json.after.title,
								department_id: institute.departments.find(
									d => d.department_name === json.before.title
								).department_id,
							};

							institute.departments = institute.departments.map(department => {
								if (department.department_id !== newDepartment.department_id) {
									return department;
								}

								return newDepartment;
							});

							collection.updateOne(
								{ _id: new ObjectId("65eb241ce29a7eb4205f6f26") },
								{ $set: document }
							);

							return;
						}
					});
				}
				// FIXME: insert
				if (!json.before && json.after) {
					document.institutes.forEach(institute => {
						if (institute.institute_id === json.after.institute_id) {
							let maxId = 0;

							document.institutes.forEach(i => {
								i.departments.forEach(department => {
									if (department.department_id > maxId) {
										maxId = department.department_id;
									}
								});
							});

							institute.departments.push({
								department_name: json.after.title,
								department_id: maxId + 1,
							});

							collection.updateOne(
								{ _id: new ObjectId("65eb241ce29a7eb4205f6f26") },
								{ $set: document }
							);
							return;
						}
					});
				}
			} catch (e) {
				console.log(e);
			}
		},
	});
};

run();
