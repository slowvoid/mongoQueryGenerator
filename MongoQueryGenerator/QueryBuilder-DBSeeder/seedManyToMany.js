const faker = require('faker')
const mongoose = require('mongoose')

let personSchema = new mongoose.Schema({
    _id: { type: Number },
    name: { type: String },
}, { collection: 'Person', versionKey: false })

let carSchema = new mongoose.Schema({
    _id: { type: Number },
    name: { type: String },
    year: { type: Number }
}, { collection: 'Car', versionKey: false })

let insCompanySchema = new mongoose.Schema({
    _id: { type: Number },
    name: { type: String }
}, { collection: 'InsCompany', versionKey: false })

let insuranceSchema = new mongoose.Schema({
    _id: { type: Number },
    personId: { type: Number },
    carId: { type: Number },
    companyId: { type: Number },
}, { collection: 'Insurance', versionKey: false })

let dbConn = mongoose.createConnection('mongodb://localhost/researchManyToMany')

let personModel = dbConn.model('Person', personSchema)
let carModel = dbConn.model('Car', carSchema)
let insCompanyModel = dbConn.model('InsCompany', insCompanySchema)
let insuranceModel = dbConn.model('Insurance', insuranceSchema)

// delete collections before running
dbConn.dropCollection('Person')
dbConn.dropCollection('Car')
dbConn.dropCollection('InsCompany')
dbConn.dropCollection('Insurance')

// person
for (let i = 0; i < 10; i++) {
    let model = new personModel({
        _id: i + 1,
        name: faker.name.firstName()
    })

    model.save()
    console.log(model)
}


let carModels = ['Astra', 'Celta', 'Fox', 'Fusca', 'Onix', 'Uno', 'Mobi']

// car
for (let i = 0; i < 10; i++) {
    let model = new carModel({
        _id: i + 1,
        name: carModels[faker.random.number({ min: 0, max: carModels.length - 1 })],
        year: faker.random.number({min: 2000, max: 2019})
    })

    model.save()
    console.log(model)
}


// ins company
for (let i = 0; i < 10; i++) {
    let model = new insCompanyModel({
        _id: i + 1,
        name: faker.company.companyName()
    })

    model.save()
    console.log(model)
}

// insurance
for (let i = 0; i < 10; i++) {
    let model = new insuranceModel({
        _id: i + 1,
        personId: faker.random.number({ min: 1, max: 10 }),
        carId: faker.random.number({ min: 1, max: 10 }),
        companyId: faker.random.number({ min: 1, max: 10 })
    })

    model.save()
    console.log(model)
}