const faker = require('faker')
const mongoose = require('mongoose')

let personSchema = mongoose.Schema({
    _id: { type: Number, required: true },
    name: String,
    cars: { type: Array }
}, { collection: 'Person', versionKey: false })

const dbConnection = mongoose.createConnection('mongodb://localhost/researchOneToManyRelationshipAttributesEmbedded')
const personModel = dbConnection.model('PersonDrivesCars', personSchema)

dbConnection.collection('Person').drop()

let carModels = ['Astra', 'Celta', 'Fox', 'Fusca', 'Onix', 'Uno', 'Mobi']

for (let i = 0; i < 10; i++) {
    let carArray = [];
    for (let j = 0; j < faker.random.number({ min: 0, max: 5 }); j++) {
        carArray.push({
            name: carModels[faker.random.number({ min: 0, max: carModels.length - 1 })],
            year: faker.random.number({min: 2000, max: 2019}),
            drivesFor: faker.name.firstName()
        })
    }
    let model = new personModel({
        _id: i + 1,
        name: faker.name.firstName(),
        cars: carArray
    })

    model.save()

    console.log(model)
}