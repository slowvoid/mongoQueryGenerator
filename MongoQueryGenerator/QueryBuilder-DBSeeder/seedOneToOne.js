const faker = require('faker')
const mongoose = require('mongoose')

let personSchema = new mongoose.Schema({
    _id: { type: Number, required: true },
    name: String,
    carId: Number
}, { collection: 'Person', versionKey: false })

let carSchema = new mongoose.Schema({
    _id: { type: Number, required: true },
    name: String,
    year: Number
}, { collection: 'Car', versionKey: false })

const dbConnection = mongoose.createConnection('mongodb://localhost:27017/researchDatabase')
const personModel = dbConnection.model('Person', personSchema)
const carModel = dbConnection.model('Car', carSchema)

dbConnection.collection('Person').drop()
dbConnection.collection('Car').drop()

for (let i = 0; i < 10; i++) {
    let model = new personModel({
        _id: i + 1,
        name: faker.name.firstName(),
        carId: faker.random.number({min: 1, max: 10})
    })

    model.save()

    console.log(model)
}

let carModels = ['Astra', 'Celta', 'Fox', 'Fusca', 'Onix', 'Uno', 'Mobi']

for (let i = 0; i < 10; i++) {
    let model = new carModel({
        _id: i + 1,
        name: carModels[faker.random.number({ min: 0, max: carModels.length - 1 })],
        year: faker.random.number({ min: 2000, max: 2019 })
    })

    model.save()

    console.log(model)
}