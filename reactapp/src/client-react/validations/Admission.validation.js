import * as yup from 'yup';

export const AdmissionValidationSchema = yup.object().shape({
    id: yup.string().matches(/^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/i),
    dateOfApplication: yup.date().required('Неверная дата'),
    passportID: yup.string().nullable(true),
    passportSeries: yup.string().nullable(true),
    passportNumber: yup.number().nullable(true),
    isTargeted: yup.bool().default(false),
    isWithoutEntranceExams: yup.bool().default(false),
    isOutOfCompetition: yup.bool().default(false),
    email: yup.string().email().nullable(true),
    student: yup.object().shape({
        id: yup.string().matches(/^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/i),
        name: yup.string().required('Введите имя'),
        surname: yup.string().required('Введите фамилию'),
        patronymic: yup.string().required('Введите отчество'),
        gpa: yup.number().min(0).required('Введите баллы'),
    }).required(),
    studentScores: yup.array().of(
        yup.object().shape({
            subject: yup.object().shape({
                name: yup.string().required()
            }),
            subjectId: yup.string().matches(/^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/i),
            score: yup.number().min(0).required('Введите баллы')
        })
    ).required(),
    specialityPriorities: yup.array().of(
        yup.object().shape({
            id: yup.string().matches(/^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/i),
            specialityName: yup.string().required(),
            priority: yup.number().min(0).required()
        })
    ).test("at-least-one-true", 'Выберите хотя бы одну специальность', (obj) => {
        return Object.values(obj).some((value) => value.priority > 0);
    }).required(),
});