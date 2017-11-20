import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import FactCard from '../../components/FactCard.jsx';

describe('ListView', () => {

    let details;
    let fetchDetails;
    let entry;
    let selectEntry;
    let selectedEntryId;
    let factCard;
    let showingDetails;
    let showDetails;

    beforeEach(() => {
        details = {};
        fetchDetails = jest.fn();
        selectEntry = jest.fn();
        showDetails = jest.fn();
        showingDetails = false;
        selectedEntryId = "abc";
        entry = {
            id: "abc",
            content: "This is a fact",
            type: "OBJECTIVE"
        }

        factCard = mount(
            <FactCard
                {...entry}
                details={details}
                fetchDetails={fetchDetails}
                selectedEntryId={selectedEntryId}
                selectEntry={selectEntry}
                showingDetails={showingDetails}
                showDetails={showDetails}
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