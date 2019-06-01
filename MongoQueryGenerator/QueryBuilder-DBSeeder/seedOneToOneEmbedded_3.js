const faker = require('faker')
const mongoose = require('mongoose')

let personSchema = new mongoose.Schema({
    _id: { type: Number, required: true },
    name: String,
    carId: { type: Number },
    carData: {type: Object}
}, { collection: 'PersonDrivesCarMixed', versionKey: false })


const dbConnection = mongoose.createConnection('mongodb://localhost:27017/researchDatabase')
const personModel = dbConnection.model('PersonDrivesCarMixed', personSchema)

dbConnection.collection('PersonDrivesCarMixed').drop()

let carModels = ['Astra', 'Celta', 'Fox', 'Fusca', 'Onix', 'Uno', 'Mobi']

for (let i = 0; i < 10; i++) {
    let carId = faker.random.number({ min: 0, max: carModels.length - 1 })
    let model = new personModel({
        _id: i + 1,
        name: faker.name.firstName(),
        carId: carId + 1,
        carData: {
            carName: carModels[carId],
            carYear: faker.random.number({ min: 2000, max: 2019 })
        }
    })

    model.save()

    console.log(model)
}