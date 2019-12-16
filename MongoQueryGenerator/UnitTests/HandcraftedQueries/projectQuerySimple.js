db.Person.aggregate([
    {
        "$project": {
            _id: false,
            name: true,
            age: true
        }
    }
]).pretty()