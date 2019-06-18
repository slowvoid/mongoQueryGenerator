const faker = require('faker')
const mongoose = require('mongoose')

let personSchema = new mongoose.Schema({
    _id: {type: Number, required: true},
    name: String
}, { collection: 'Person', versionKey: false})

let carSchema = new mongoose.Schema({
    _id: {type: Number, required: true},
    name: String,
    driverId: Number,
    year: Number,
    drivesFor: String
}, { collection: 'Car', versionKey: false})

const dbConnection = mongoose.createConnection('mongodb://localhost:27017/researchOneToManyRelationshipAttributes')
const personModel = dbConnection.model('Person', personSchema)
const carModel = dbConnection.model('Car', carSchema)

dbConnection.collection('Person').drop()
dbConnection.collection('Car').drop()

let carModels = ['Astra', 'Celta', 'Fox', 'Fusca', 'Onix', 'Uno', 'Mobi']
let carEngines = ['1.0', '1.3', '1.4', '1.5', '1.6', '1.8', '2.0']
let fuelType = ['Flex', 'Gas', 'Ethanol']

for ( let i = 0; i < 10; i++ ) {
    let model = new personModel({
        _id: i + 1,
        name: faker.name.firstName()
    })

    model.save()
    console.log(model)
}

for ( let i = 0; i < 10; i++ ) {
    let model = new carModel({
        _id: i + 1,
        driverId: faker.random.number({min: 1, max: 10}),
        name: carModels[faker.random.number({min: 0, max: carModels.length - 1})],
        year: faker.random.number({min: 2000, max: 2019}),
        drivesFor: faker.name.firstName()
    })

    model.save()
    console.log(model)
}