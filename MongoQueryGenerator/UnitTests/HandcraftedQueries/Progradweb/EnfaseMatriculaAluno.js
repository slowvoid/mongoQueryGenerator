db.Enfase.aggregate([
    {"$lookup": {
        from: 'Matricula',
        let: {
            enfaseId: '$_id'
        },
        pipeline: [
            {$match: {
                $expr: {
                    $eq: ['$codenf_matr', '$$enfaseId']
                }
            }},
            {$lookup: {
                from: 'Aluno',
                foreignField: '_id',
                localField: 'codalu_matr',
                as: 'data_Aluno'
            }},
            {$addFields: {
                data_Vinculo: {$map:{
                    input: '$data_Aluno',
                    as: 'aluno',
                    in: {
                        Aluno_codalu_alug: '$$aluno._id',
                        Aluno_nomealu_alug: '$$aluno.nomealu_alug',
                        Aluno_cpf_alug: '$$aluno.cpf_alug'
                    }
                }},
                Matricula_codmatr_matr: '$_id',
                Matricula_anoini_matr: '$anoini_matr',
                Matricula_semiini_matr: '$semiini_matr'
            }},
            {$project: {
                anoini_matr: false,
                semiini_matr: false,
                codalu_matr: false,
                codenf_matr: false,
                data_Aluno: false,
                _id: false
            }}
        ],
        as: 'data_VinculoEnfase'
    }
    },
    {
        $project: {
            codcur_enf: false
        } }
]).pretty();