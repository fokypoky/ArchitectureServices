const { Kafka } = require("kafkajs");
const { createClient } = require("redis");
require("dotenv").config();

const kafka = new Kafka({
	clientId: "redis-transformer",
	brokers: [`${process.env.BROKER_HOST}:${process.env.BROKER_PORT}`],
});

const client = createClient({
	socket: {
		host: `${process.env.REDIS_HOST}`,
		port: `${process.env.REDIS_PORT}`,
	},
});

const consumer = kafka.consumer({ groupId: "redis-data-transform-group" });

const run = async () => {
	await client.connect();
	await consumer.connect();
	await consumer.subscribe({
		topics: ["pg.public.students"],
		fromBeginning: false,
	});

	await consumer.run({
		eachMessage: async ({ message }) => {
			try {
				const json = JSON.parse(message.value.toString());

				// удаление
				if (!json?.payload?.after) {
					await client.del(json.payload.before.passbook_number);
				} else {
					const newValue = `${json.payload.after.name};${json.payload.after.group_id}`;
					await client.set(json.payload.after.passbook_number, newValue);
				}
			} catch (e) {
				console.log(e);
			}
		},
	});
};

run();
