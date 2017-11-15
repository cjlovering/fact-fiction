import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import FactCard from '../../components/FactCard.jsx';

describe('ListView', () => {

    let entry;
    let selectEntry;
    let selectedEntryId;
    let factCard;

    beforeEach(() => {
        selectEntry = jest.fn();
        selectedEntryId = "abc";
        entry = {
            id: "abc",
            content: "This is a fact",
            type: "OBJECTIVE"
        }

        factCard = mount(
            <FactCard
                {...entry}
                selectedEntryId={selectedEntryId}
                selectEntry={selectEntry}
            />);
    });

    it('FactCard requires content prop', () => {
        expect(factCard.props().content).toBeDefined();
    });

    it('FactCard requires selectEntry prop', () => {
        expect(factCard.props().selectEntry).toBeDefined();
    });

    it('FactCard requires selectedEntryId prop', () => {
        expect(factCard.props().selectedEntryId).toBeDefined();
    });

    it('Click selects the entry', () => {
        factCard.simulate('click');
        expect(selectEntry).toBeCalledWith("abc");
    });

});