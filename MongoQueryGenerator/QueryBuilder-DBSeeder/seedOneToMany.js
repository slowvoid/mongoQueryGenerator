const faker = require('faker')
const mongoose = require('mongoose')

let personSchema = mongoose.Schema({
    _id: { type: Number, required: true },
    name: String
}, { collection: 'Person', versionKey: false })

let carSchema = mongoose.Schema({
    _id: { type: Number, required: true },
    name: String,
    year: Number,
    personId: Number
}, { collection: 'Car', versionKey: false })

const dbConnection = mongoose.createConnection('mongodb://localhost/researchDatabaseOneToMany')
const personModel = dbConnection.model('Person', personSchema)
const carModel = dbConnection.model('Car', carSchema)

dbConnection.collection('Person').drop()
dbConnection.collection('Car').drop()

for (let i = 0; i < 10; i++) {
    let model = new personModel({
        _id: i + 1,
        name: faker.name.firstName()
    })

    model.save()

    console.log(model)
}

let carModels = ['Astra', 'Celta', 'Fox', 'Fusca', 'Onix', 'Uno', 'Mobi']

for (let i = 0; i < 10; i++) {
    let model = new carModel({
        _id: i + 1,
        name: carModels[faker.random.number({ min: 0, max: carModels.length - 1 })],
        year: faker.random.number({ min: 2000, max: 2019 }),
        personId: faker.random.number({min: 1, max: 10})
    })

    model.save()

    console.log(model)
}