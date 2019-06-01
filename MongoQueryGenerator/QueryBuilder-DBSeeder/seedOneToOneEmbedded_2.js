const faker = require('faker')
const mongoose = require('mongoose')

let personSchema = new mongoose.Schema({
    _id: { type: Number, required: true },
    name: String,
    carId: { type: Number },
    carName: { type: String },
    carYear: {type: Number}
}, { collection: 'PersonDrivesCar', versionKey: false })


const dbConnection = mongoose.createConnection('mongodb://localhost:27017/researchDatabase')
const personModel = dbConnection.model('PersonDrivesCar', personSchema)

dbConnection.collection('PersonDrivesCar').drop()

let carModels = ['Astra', 'Celta', 'Fox', 'Fusca', 'Onix', 'Uno', 'Mobi']

for (let i = 0; i < 10; i++) {
    let carId = faker.random.number({ min: 0, max: carModels.length - 1 })
    let model = new personModel({
        _id: i + 1,
        name: faker.name.firstName(),
        carId: carId + 1,
        carName: carModels[carId],
        carYear: faker.random.number({ min: 2000, max: 2019 })
    })

    model.save()

    console.log(model)
}