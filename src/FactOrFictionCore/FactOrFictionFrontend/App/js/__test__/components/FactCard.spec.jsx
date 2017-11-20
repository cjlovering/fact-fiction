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
    let castVote;
    let sentenceVote;
    beforeEach(() => {
        details = {};
        fetchDetails = jest.fn();
        selectEntry = jest.fn();
        showDetails = jest.fn();
        showingDetails = false;
        castVote = jest.fn();
        sentenceVote = "TRUE";
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
                sentenceVote={sentenceVote}
                showingDetails={showingDetails}
                showDetails={showDetails}
                castVote={castVote}
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

    it('FactCard requires castVote prop', () => {
        expect(factCard.props().castVote).toBeDefined();
    });

    it('Click selects the entry', () => {
        factCard.simulate('click');
        expect(selectEntry).toBeCalledWith("abc");
    });

});