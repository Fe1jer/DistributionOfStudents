import * as yup from 'yup';

export const GroupOfSpecialitiesValidationSchema = yup.object().shape({
    group: yup.object().shape({
        id: yup.number(),
        name: yup.string().required('Введите название'),
        startDate: yup.date().required('Неверная дата'),
        enrollmentDate: yup.date().required('Неверная дата'),
        description: yup.string().nullable(true),
        isCompleted: yup.boolean().default(false),
        formOfEducation: yup.object().shape({
            id: yup.number(),
            year: yup.number(),
            isDailyForm: yup.boolean().required(),
            isBudget: yup.boolean().required(),
            isFullTime: yup.boolean().required()
        }).required(),
    }).required(),
    selectedSpecialities: yup.array().of(
        yup.object().shape({
            specialityName: yup.string().required(),
            specialityId: yup.number().min(0).required(),
            isSelected: yup.boolean().default(false)
        })
    ).test("at-least-one-true", 'Выберите хотя бы одну специальность', (obj) => {
        return Object.values(obj).some((value) => value.isSelected);
    }).required(),
    selectedSubjects: yup.array().of(
        yup.object().shape({
            subject: yup.string().required(),
            subjectId: yup.number().min(0).required(),
            isSelected: yup.boolean().default(false)
        })
    ).test("at-least-one-true", 'Выберите хотя бы один предмет', (obj) => {
        return Object.values(obj).some((value) => value.isSelected);
    }).required(),
});