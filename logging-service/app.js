const { Kafka } = require("kafkajs");
require("dotenv").config();

const kafka = new Kafka({
	clientId: "logging-service-client",
	brokers: [`${process.env.BROKER_HOST}:${process.env.BROKER_PORT}`],
});

const consumer = kafka.consumer({ groupId: "logging-group" });

const run = async () => {
	await consumer.connect();
	await consumer.subscribe({
		topics: [
			"pg.public.courses",
			"pg.public.departments",
			"pg.public.groups",
			"pg.public.lecture_materials",
			"pg.public.lecture_types",
			"pg.public.lectures",
			"pg.public.specialities",
			"pg.public.students",
			"pg.public.timetable",
			"pg.public.visits",
			"pg.public.visits_202305",
			"pg.public.visits_202306",
		],
		fromBeginning: false,
	});

	await consumer.run({
		eachMessage: async ({ message, topic }) => {
			try {
				const json = JSON.parse(message.value.toString());

				const id = json.payload.before?.id ?? json.payload.after?.id;

				const log = {
					before: json.payload.before ? json.payload.before : "null",
					after: json.payload.after ? json.payload.after : "null",
				};

				let url = `http://${process.env.ELASTIC_SEARCH_URL}:${process.env.ELASTIC_SEARCH_PORT}/${topic}/_doc/${id}_${message.timestamp}`;
				// TODO: пофиксить название документа для топиков visits. у них нет id 
				
				fetch(url, {
					method: "POST",
					headers: { "Content-Type": "application/json" },
					body: JSON.stringify({
						before: JSON.stringify(log.before),
						after: JSON.stringify(log.after),
					}),
				})
					.then(r => r.text())
					.then(text => console.log(text));
			} catch (e) {
				console.error(e);
			}
		},
	});
};

run();
