db.Person.aggregate([
    {"$match": {
        $expr: {
            $lte: ['$age', 27]
        }
    }}
]).pretty()