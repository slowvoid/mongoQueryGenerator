const faker = require('faker')
const mongoose = require('mongoose')

let personSchema = new mongoose.Schema({
    _id: { type: Number, required: true },
    name: String,
    car: Object,
    insuranceId: Number
}, { collection: 'Person', versionKey: false })

let insuranceSchema = new mongoose.Schema({
    _id: { type: Number, required: true },
    name: String
}, { collection: 'Insurance', versionKey: false })

const dbConnection = mongoose.createConnection('mongodb://localhost:27017/researchOneToOneMultipleMixed')
const personModel = dbConnection.model('Person', personSchema)
const insuranceModel = dbConnection.model('Insurance', insuranceSchema)

dbConnection.collection('Person').drop()
dbConnection.collection('Insurance').drop()

let carModels = ['Astra', 'Celta', 'Fox', 'Fusca', 'Onix', 'Uno', 'Mobi']

for ( let i = 0; i < 10; i++ ) {
    let model = new personModel({
        _id: i + 1,
        name: faker.name.firstName(),
        car: {
            name: carModels[faker.random.number({min: 0, max: carModels.length - 1})],
            year: faker.random.number({min: 2000, max: 2019})
        },
        insuranceId: faker.random.number({min: 1, max: 10})
    })

    model.save()

    console.log(model)
}

for ( let i = 0; i < 10; i++ ) {
    let model = new insuranceModel({
        _id: i + 1,
        name: faker.company.companyName()
    })

    model.save()

    console.log(model)
}